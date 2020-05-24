# Blazor Azure Deploy

![](https://placehold.it/15/4747d1/000000?text=+) 
If you like my blazor works and want to see more open sourced repos please support me with [paypal donation](https://www.paypal.me/VakhtangiAbashidze/10)
![](https://placehold.it/15/4747d1/000000?text=+) 

![](https://placehold.it/15/00e600/000000?text=+) 
Please send [email](mailto:VakhtangiAbashidze@gmail.com) if you consider to **hire me**.
![](https://placehold.it/15/00e600/000000?text=+)     


![](https://placehold.it/15/ffffff/000000?text=+)  

This is console application convenient utility for blazor webassembly fast deploy to azure storage static site.

After publishing app we can use it to upload files to storage, also will gzip files and set content types, encoding and do some other good stuff.

We won't need anymore to use Visual studio code for deploy.

Usage: clone repo, build locally, run cmd with this command

BlazorAzureDeploy.exe -d E:\tourfolder\wwwroot -e .dll .wasm .js .png .jpg .css .json .pdb .dat .ico .map  -f $web -a yourstoragename -k yourstoragekey -z true

You can check options and code to see how it works.

More detailed documentation will be added later.


PR's are very welcome.
