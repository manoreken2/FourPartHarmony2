
import numpy as np
from scipy.io import wavfile
import matplotlib.pyplot as plt
import scipy.fftpack

def show_info(aname, a):
    print ("Array", aname)
    print ("shape:", a.shape)
    print ("dtype:", a.dtype)
    print( "min, max:", a.min(), a.max())
    print()


#70Hzと74Hzの区別がつく分解能が要る。
FR=44100
T=1.0/FR
N=16384

rate, data = wavfile.read('C:/work/TestE.wav',True)
data=np.resize(data,N)
#show_info("data", data)

y=data.astype(np.float)/32768

#show_info("y", y)

yf = scipy.fftpack.fft(y)

#show_info("yf",yf)

#70Hzと4kHzの間だけを見る。
startIdx=(int)(70*N/FR)
endIdx=(int)(4000*N/FR)

print("s=:",+startIdx,endIdx)

yf=yf[startIdx:endIdx]
inData=np.abs(yf)

show_info("in",inData)

'''
xf = np.linspace(0.0, 1.0/(2.0*T), N/2)
fig, ax = plt.subplots()
ax.plot(xf, 2.0/N * np.abs(yf[:N//2]))
plt.show()
'''


