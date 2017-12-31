#!/usr/bin/python3

import GenerateDataset2
import Learn
import sys

labelPath = 'chordNames.txt'
modelPath = 'model.sav'

if __name__ == '__main__':
    inputWavname = sys.argv[-1]
    Learn.predict(modelPath, labelPath, inputWavname)


