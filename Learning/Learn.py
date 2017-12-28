from pandas import read_table
import numpy as np
import matplotlib.pyplot as plt
from os import walk
import numpy as np
import matplotlib.pyplot as plt
import scipy.fftpack
from scipy.io import wavfile

chordNamesPath = 'chordNames.txt'
fftCsvPath = 'chordFft.csv'

# =====================================================================

def read_data():
    frame = read_table(
        fftCsvPath,
        encoding='utf-8',
        sep=',',
        skipinitialspace=True,
        index_col=None,
        header=None,
    )
    return frame

# =====================================================================


def get_features_and_labels(frame):
    arr = np.array(frame, dtype=np.float)

    # first column == target value
    X, y = arr[:, 1:], arr[:, 0]
    
    # Use 90% of the data for training; test against the rest
    from sklearn.model_selection import train_test_split
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.1)

    # sklearn.pipeline.make_pipeline could also be used to chain 
    # processing and classification into a black box, but here we do
    # them separately.
    
    # If values are missing we could impute them from the training data
    #from sklearn.preprocessing import Imputer
    #imputer = Imputer(strategy='mean')
    #imputer.fit(X_train)
    #X_train = imputer.transform(X_train)
    #X_test = imputer.transform(X_test)
    
    # Normalize the attribute values to mean=0 and variance=1
    from sklearn.preprocessing import StandardScaler
    scaler = StandardScaler()
    # To scale to a specified range, use MinMaxScaler
    #from sklearn.preprocessing import MinMaxScaler
    #scaler = MinMaxScaler(feature_range=(0, 1))
    
    # Fit the scaler based on the training data, then apply the same
    # scaling to both training and test sets.
    scaler.fit(X_train)
    X_train = scaler.transform(X_train)
    X_test = scaler.transform(X_test)

    # Return the training and test sets
    return X_train, X_test, y_train, y_test


# =====================================================================


def evaluate_classifier(X_train, X_test, y_train, y_test):
    '''
    Run multiple times with different classifiers to get an idea of the
    relative performance of each configuration.

    Returns a sequence of tuples containing:
        (title, precision, recall)
    for each learner.
    '''

    # Import some classifiers to test
    from sklearn.svm import LinearSVC, NuSVC
    from sklearn.ensemble import AdaBoostClassifier

    # We will calculate the P-R curve for each classifier
    from sklearn.metrics import precision_recall_curve, f1_score
    
    # Here we create classifiers with default parameters. These need
    # to be adjusted to obtain optimal performance on your data set.
    
    # Test the linear support vector classifier
    classifier = LinearSVC(C=1)
    # Fit the classifier
    classifier.fit(X_train, y_train)
    score = f1_score(y_test, classifier.predict(X_test))
    # Generate the P-R curve
    y_prob = classifier.decision_function(X_test)
    precision, recall, _ = precision_recall_curve(y_test, y_prob)
    # Include the score in the title
    yield 'Linear SVC (F1 score={:.3f})'.format(score), precision, recall

    # Test the Nu support vector classifier
    classifier = NuSVC(kernel='rbf', nu=0.5, gamma=1e-3)
    # Fit the classifier
    classifier.fit(X_train, y_train)
    score = f1_score(y_test, classifier.predict(X_test))
    # Generate the P-R curve
    y_prob = classifier.decision_function(X_test)
    precision, recall, _ = precision_recall_curve(y_test, y_prob)
    # Include the score in the title
    yield 'NuSVC (F1 score={:.3f})'.format(score), precision, recall

    # Test the Ada boost classifier
    classifier = AdaBoostClassifier(n_estimators=50, learning_rate=1.0, algorithm='SAMME.R')
    # Fit the classifier
    classifier.fit(X_train, y_train)
    score = f1_score(y_test, classifier.predict(X_test))
    # Generate the P-R curve
    y_prob = classifier.decision_function(X_test)
    precision, recall, _ = precision_recall_curve(y_test, y_prob)
    # Include the score in the title
    yield 'Ada Boost (F1 score={:.3f})'.format(score), precision, recall

# =====================================================================


def plot(results):
    '''
    Create a plot comparing multiple learners.

    `results` is a list of tuples containing:
        (title, precision, recall)
    
    All the elements in results will be plotted.
    '''

    # Plot the precision-recall curves

    fig = plt.figure(figsize=(6, 6))
    fig.canvas.set_window_title('Classifying data from ' + URL)

    for label, precision, recall in results:
        plt.plot(recall, precision, label=label)

    plt.title('Precision-Recall Curves')
    plt.xlabel('Precision')
    plt.ylabel('Recall')
    plt.legend(loc='lower left')

    # Let matplotlib improve the layout
    plt.tight_layout()

    # ==================================
    # Display the plot in interactive UI
    plt.show()

    # To save the plot to an image file, use savefig()
    #plt.savefig('plot.png')

    # Open the image file with the default image viewer
    #import subprocess
    #subprocess.Popen('plot.png', shell=True)

    # To save the plot to an image in memory, use BytesIO and savefig()
    # This can then be written to any stream-like object, such as a
    # file or HTTP response.
    #from io import BytesIO
    #img_stream = BytesIO()
    #plt.savefig(img_stream, fmt='png')
    #img_bytes = img_stream.getvalue()
    #print('Image is {} bytes - {!r}'.format(len(img_bytes), img_bytes[:8] + b'...'))

    # Closing the figure allows matplotlib to release the memory used.
    plt.close()

def wav_to_freq(path):
    #70Hzと74Hzの区別がつく分解能が要る。
    FR=44100
    T=1.0/FR
    N=16384

    rate, data = wavfile.read(path,True)
    data=np.resize(data,N)

    y=data.astype(np.float)/32768

    yf = scipy.fftpack.fft(y)

    #70Hzと4kHzの間だけを見る。
    startIdx=(int)(70*N/FR)
    endIdx=(int)(4000*N/FR)

    yf=yf[startIdx:endIdx]
    result=np.abs(yf)
    return result

def create_csv():
    f = []
    for (dirpath, dirnames, filenames) in walk('wav'):
        f.extend(filenames)

    i=0
    numberToChordName = {}
    chordNameToNumber = {}
    for fname in f:
        name = fname.split('_')
        if name[0] not in chordNameToNumber:
            chordNameToNumber[ name[0] ] = i
            numberToChordName[ i ] = name[0]
            i = i + 1

    #print("", chordNameToNumber)
    i=0
    fp = open(chordNamesPath, 'w')
    for cname in chordNameToNumber:
        fp.write(cname)
        fp.write('\n')
        i = i + 1
    fp.close()

    fp = open(fftCsvPath, 'w')
    for fname in f:
        name = fname.split('_')
        path = 'wav/' + fname
        freq = wav_to_freq(path)
        fp.write('%d' % (chordNameToNumber[ name[0] ]))
        for v in freq:
            fp.write(',%f' % (v))
        fp.write('\n')
    fp.close()

# =====================================================================


if __name__ == '__main__':
    # create csv files
    #create_csv()

    # read csv file created by create_csv()
    print('reading file...')
    frame = read_data()

    # Process data into feature and label arrays
    print("Processing {} samples with {} attributes".format(len(frame.index), len(frame.columns)))
    X_train, X_test, y_train, y_test = get_features_and_labels(frame)

    #print('', y_test)
    #print('', X_test[0])

    # Evaluate multiple classifiers on the data
    print("Evaluating classifiers")
    results = list(evaluate_classifier(X_train, X_test, y_train, y_test))

    # Display the results
    print("Plotting the results")
    plot(results)
