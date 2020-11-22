# DotToDat
A simple converter in C# changing graphs coded in .dot files to .dat files that may be used with GNU MathProg solvers

# Example .dot file

```
graph G {
0;
1;
2;
3;
4;
5;
0 -- 1;
0 -- 2;
1 -- 2;
1 -- 4;
3 -- 2;
3 -- 5;
4 -- 5;
}
```

# Example .dat file after conversion

```
data;
param n := 6;
set E := 0 1 0 2 1 2 1 4 3 2 3 5 4 5;
end;
```

# Commands 

```
c:\>DotToDat.exe test.dot
c:\>Data successfully saved as test.dat
```




