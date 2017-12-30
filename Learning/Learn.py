#!/usr/bin/python

from pandas import read_table
import numpy as np
import pickle
from sklearn.svm import LinearSVC
from sklearn.metrics import f1_score

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

def train_svc(X_train, X_test, y_train, y_test):
    classifier = LinearSVC(C=1)
    classifier.fit(X_train, y_train)
    y_pred = classifier.predict(X_test)
    score = f1_score(y_test, y_pred, average='weighted')
    return score

def train(datasetPath):
    print('reading file...')
    frame = read_data(datasetPath)

    # Process data into feature and label arrays
    print("Processing {} samples with {} attributes".format(len(frame.index), len(frame.columns)))
    X_train, X_test, y_train, y_test = get_features_and_labels(frame)

    #print('', y_test)
    #print('', X_test[0])

    print('training...')
    results = train_svc(X_train, X_test, y_train, y_test)

    print('score = ', results)
