
def ConcatFiles(inFilenames, outPath):
    with open(outPath, 'w') as outfile:
        for fname in inFilenames:
            with open(fname) as infile:
                for line in infile:
                    outfile.write(line)

                    
