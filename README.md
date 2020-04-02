# BlazorAzureDeploy


This is console application convenient utility for blazor webassembly fast deploy to azure storage static site.

After publishing app from Visual studio we can use it to upload files to storage, also will gzip files and set content types, encodyng and do some other good stuff.

We won't need anymore to use Visual studio code for deploy.

Usage: clone repo, build locally, run cmd with this command

BlazorAzureDeploy.exe -d E:\wwwroot -e .dll .wasm .js .png .jpg .css .json .pdb  -f $web -a yourstoragename -k yourstoragekey -z true

You can check options and code to see how it works.

More detailed documentation will be added later.
