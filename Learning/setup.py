from cx_Freeze import setup, Executable

base = None


executables = [Executable("RunPredict.py", base=base)]

setup(
    name = "RunPredict",
    version = "1.0.0",
    description = 'RunPredict',
    executables = executables
)