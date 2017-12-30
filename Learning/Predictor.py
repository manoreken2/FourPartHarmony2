#!/usr/bin/python
# 日本語


import pickle
from sklearn.svm import LinearSVC
from sklearn.metrics import f1_score

class Predictor(object):
    def __init__(self):
        self.classifier = LinearSVC(C=1)

    def learn(self, X_train, y_train):
        self.classifier.fit(X_train, y_train)

    def predict(self, x):
        return self.classifier.predict(x)

    def save(self, path):
        pickle.dump(self.classifier, open(path, 'wb'))

    def load(self, path):
        self.classifier = pickle.load(open(path, 'rb'))


