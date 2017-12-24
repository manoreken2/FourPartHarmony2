#!/bin/bash
for i in $(ls midi); do
  echo $i;
  f="${i%.*}"
  /cygdrive/c/Program\ Files/Modartt/Pianoteq\ 6\ STAGE/Pianoteq\ 6\ STAGE.exe --headless  --midi midi/$i --wav wav/$f.wav;
done
 
