@echo off
@ftp -i -s:"%~f0"
open tso2.telia.se
ac84mre
Lucky123
lcd "P:\Pricing Planner\Dev\PPV-new"
cd 'AQ66.UTVP.U1.PPVIN'
PUT PPVDATA.TXT PCFIL
cd 'AQ66.UTIWY.JOBLIB'
PUT JCL.TXT IWTEST
