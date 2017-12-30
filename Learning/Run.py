#!/usr/bin/python

import GenerateDataset2
import Learn

labelPath = 'chordNames.txt'
datasetPath = 'chordFft.csv'
wavDirPath = 'wav'

if __name__ == '__main__':
    #GenerateDataset2.generate(wavDirPath, labelPath, datasetPath)
    
    Learn.train(datasetPath)


