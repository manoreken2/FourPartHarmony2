#!/usr/bin/python

import GenerateDataset2
import Learn

testWavPath = 'funauta.wav'
labelPath = 'chordNames.txt'
modelPath = 'model.sav'

trainDatasetPath = 'chordFft.csv'
trainWavDirPath = 'wav'

if __name__ == '__main__':
    #GenerateDataset2.generate(trainWavDirPath, labelPath, trainDatasetPath)
    
    #Learn.train(trainDatasetPath, modelPath)

    Learn.predict(modelPath, labelPath, testWavPath)


