#!/usr/bin/python3

import GenerateDataset2
import Learn
from ConcatFiles import ConcatFiles

testWavPath = 'wavStein/A_0.wav'
labelPath = 'chordNames.txt'
modelPath = 'model.sav'

if __name__ == '__main__':
    GenerateDataset2.generate('wavStein', labelPath, 'trainStein.csv')
    GenerateDataset2.generate('wavK2', 'chordNamesK2.txt', 'trainK2.csv')
    ConcatFiles(['trainStein.csv', 'trainK2.csv'], 'train.csv')
    Learn.train('train.csv', 'model.sav')
    #Learn.train('trainStein.csv', 'modelStein.sav')

    #Learn.predict(modelPath, labelPath, testWavPath)


