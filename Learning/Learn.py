#!/usr/bin/python

from pandas import read_table
import numpy as np
import pickle
from sklearn.svm import LinearSVC
from sklearn.metrics import f1_score
from Predictor import Predictor
from scipy.io import wavfile
import scipy.fftpack


def normalizePositiveArray(a):
    vMax = max(a)
    if vMax < 0.0001:
        vMax = 1.0
    return a / vMax

def normalizeArray(a):
    vMax = max(np.abs(a))
    if vMax < 0.0001:
        vMax = 1.0
    return a / vMax

def read_data(datasetPath):
    frame = read_table(
        datasetPath,
        encoding='utf-8',
        sep=',',
        skipinitialspace=True,
        index_col=None,
        header=None,
    )
    return frame

def get_features_and_labels(frame):
    arr = np.array(frame, dtype=np.float)

    # first column == target value
    X, y = arr[:, 1:], arr[:, 0]
    
    # Use 90% of the data for training; test against the rest
    from sklearn.model_selection import train_test_split
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.1)

    '''
    # Normalize the attribute values to mean=0 and variance=1
    from sklearn.preprocessing import StandardScaler
    scaler = StandardScaler()
    scaler.fit(X_train)
    X_train = scaler.transform(X_train)
    X_test = scaler.transform(X_test)
    '''

    return X_train, X_test, y_train, y_test

def train_svc(X_train, X_test, y_train, y_test, savePath):
    classifier = LinearSVC(C=1)
    classifier.fit(X_train, y_train)
    pickle.dump(classifier, open(savePath, 'wb'))
    y_pred = classifier.predict(X_test)
    score = f1_score(y_test, y_pred, average='weighted')
    return score

def train(datasetPath, modelSavePath):
    print('reading file...')
    frame = read_data(datasetPath)

    # Process data into feature and label arrays
    print("Processing {} samples with {} attributes".format(len(frame.index), len(frame.columns)))
    X_train, X_test, y_train, y_test = get_features_and_labels(frame)

    #print('', y_test)
    #print('', X_test[0])

    print('training...')
    pred = Predictor()
    pred.learn(X_train, y_train)
    
    y_pred = pred.predict(X_test)
    score = f1_score(y_test, y_pred, average='weighted')
    print('score = ', score)

    pred.save(modelSavePath)

def predict(modelPath, labelPath, wavPath):
    lf = open(labelPath, 'r')
    labelLn = lf.readlines()

    # remove \n
    label = []
    for item in labelLn:
        label.append(item.strip())

    pred = Predictor()
    pred.load(modelPath)

    #70Hzと74Hzの区別がつく分解能が要る。
    FR=44100
    T=1.0/FR
    N=16384

    rate, dataAll = wavfile.read(wavPath,True)

    nPred = (int)(len(dataAll) / N)
    for t in range(0,nPred):
        data = dataAll[t*N:(t+1)*N]

        # 16bit int format -> float64
        x=data.astype(np.float)

        # normalize input time domain data to [-1 +1]
        xs = normalizeArray(x)
    
        w = scipy.signal.blackman(N)
        xf = scipy.fftpack.fft(xs * w)
        xfa = np.abs(xf)

        #70Hzと4kHzの区間を見る。
        startIdx=(int)(70*N/FR)
        endIdx=(int)(4000*N/FR)
        xfs=xfa[startIdx:endIdx]

        # normalize frequency domain data to [0, +1]
        xIn = normalizePositiveArray(xfs)

        xInR = np.reshape(xIn,(1,-1))
        ypred = pred.predict(xInR)

        ypredI = int(ypred)

        #print('', ypredI)
        print('', label[ypredI])

