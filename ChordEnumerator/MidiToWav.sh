#!/bin/bash
for i in $(ls midi); do
  echo $i;
  f="${i%.*}"
  /cygdrive/c/Program\ Files/Modartt/Pianoteq\ 6\ STAGE/Pianoteq\ 6\ STAGE.exe --headless --preset "Steinway D Cinematic" --mono --midi midi/$i --wav tmp.wav;
  sox tmp.wav wav/$f.wav trim 0.5;
done
 
