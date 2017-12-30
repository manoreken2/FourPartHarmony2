#!/usr/bin/python
# 日本語

import numpy as np
import scipy.fftpack
import scipy.signal
from scipy.io import wavfile
from os import walk
import sklearn.preprocessing

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

def wav_start_to_freq(path):
    #70Hzと74Hzの区別がつく分解能が要る。
    FR=44100
    T=1.0/FR
    N=16384

    rate, data = wavfile.read(path,True)
    data=np.resize(data,N)

    # 16bit int format -> float64
    y=data.astype(np.float)

    # normalize input time domain data to [-1 +1]
    ys = normalizeArray(y)
    
    w = scipy.signal.blackman(N)
    yf = scipy.fftpack.fft(ys * w)
    yfa = np.abs(yf)

    #70Hzと4kHzの区間を見る。
    startIdx=(int)(70*N/FR)
    endIdx=(int)(4000*N/FR)
    yfs=yfa[startIdx:endIdx]

    # normalize frequency domain data to [0, +1]
    result = normalizePositiveArray(yfs)

    return result

def generate(wavDirPath, labelPath, datasetPath):
    f = []
    for (dirpath, dirnames, filenames) in walk(wavDirPath):
        f.extend(filenames)

    i=0
    numberToChordName = {}
    chordNameToNumber = {}
    for fname in f:
        name = fname.split('_')
        if name[0] not in chordNameToNumber:
            chordNameToNumber[ name[0] ] = i
            numberToChordName[ i ] = name[0]
            i += 1

    i=0
    fp = open(labelPath, 'w')
    for cname in chordNameToNumber:
        fp.write(cname)
        fp.write('\n')
        i += 1
    fp.close()

    i=0
    fp = open(datasetPath, 'w')
    for fname in f:
        print('\r' + str((int)(i * 100 / (len(f)-1))) + ' % ', end='') 
        name = fname.split('_')
        path = wavDirPath + '/' + fname
        freq = wav_start_to_freq(path)
        fp.write('%d' % (chordNameToNumber[ name[0] ]))
        for v in freq:
            fp.write(',%f' % (v))
        fp.write('\n')
        i += 1
    fp.close()
    print('done')

