<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
- <a href="#services-description" target="_self">Title</a> <br>
- <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
- <a href="#weather" target="_self">Weather Forecast</a> <br>
- <a href="#account" target="_self">Account</a> <br>
- <a href="#subject" target="_self">Subject</a> <br>
- <a href="#api" target="_self">MAS API</a> <br>

<h2 id="weather">Weather Forecast <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``GET /api/mas/v1/weather-forecast ``  
>   ***Description***: Default api  
>   ***Role Access***: Admin  

<h2 id="account">Account <a href="#table-of-contents" target="_self">🔙</a></h2>

> - ``POST /api/mas/v1/accounts/login-admin ``  
>   ***Description***: Login with Admin Account   
>
> - ``POST /api/mas/v1/accounts/login-google ``  
>   ***Description***: Login User with Google Account  
>
> - ``POST /api/mas/v1/accounts/register-admin ``  
>   ***Description***: Register Admin Account   
>

<h2 id="subject">Subject  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/subjects ``  
>   ***Description***: Get all Subjects   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by search string.
>
> - ``POST /api/mas/v1/subjects/ ``  
>   ***Description***: Create a Subject   
>   ***Role Access***: Admin  
>
> - ``GET /api/mas/v1/subjects/{subjectId} ``  
>   ***Description***: Get Subject by Id   
>   ***Role Access***: Admin, User  
>
> - ``PUT /api/mas/v1/subjects/{subjectId} ``  
>   ***Description***: Update Subject   
>   ***Role Access***: Admin  
>
> - ``DELETE /api/mas/v1/subjects/{subjectId} ``  
>   ***Description***: Delete Subject by Id   
>   ***Role Access***: Admin  
>
<h2 id="api">MAS API  <a href="#table-of-contents" target="_self">🔙</a></h2>   
