<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
- <a href="#services-description" target="_self">Title</a> <br>
- <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
- <a href="#weather" target="_self">Weather Forecast</a> <br>
- <a href="#account" target="_self">Account</a> <br>
- <a href="#major" target="_self">Major</a> <br>
- <a href="#subject" target="_self">Subject</a> <br>
- <a href="#subjectMentor" target="_self">Subject of Mentor</a> <br>
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

<h2 id="major">Major  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/majors ``  
>   ***Description***: Get all Majors   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by search string.
>
> - ``POST /api/mas/v1/majors/ ``  
>   ***Description***: Create a Major   
>   ***Role Access***: Admin  
>
> - ``GET /api/mas/v1/majors/{majorId} ``  
>   ***Description***: Get Major by Id   
>   ***Role Access***: Admin, User  
>
> - ``PUT /api/mas/v1/majors/{majorId} ``  
>   ***Description***: Update Major   
>   ***Role Access***: Admin  
>
> - ``DELETE /api/mas/v1/majors/{majorId} ``  
>   ***Description***: Delete Major by Id   
>   ***Role Access***: Admin  
>

<h2 id="subject">Subject  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/subjects ``  
>   ***Description***: Get all Subjects   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by search string, major (major ID), sort by title ASC or DESC.  
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

<h2 id="subjectMentor">Subject of Mentor  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/mentor-subjects/{mentorId} ``  
>   ***Description***: Get all Subjects of a Mentor   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging  
>
> - ``POST /api/mas/v1/mentor-subjects/ ``  
>   ***Description***: Register a Subject into Mentor Profile   
>   ***Role Access***: User(Mentor)  
>
> - ``PUT /api/mas/v1/mentor-subjects/{subjectOfMentorId} ``  
>   ***Description***: Update Subject Info of Mentor   
>   ***Role Access***: User  
>
> - ``DELETE /api/mas/v1/mentor-subjects/{subjectOfMentorId} ``  
>   ***Description***: Delete a Subject of Mentor  
>   ***Role Access***: User  
>
<h2 id="api">MAS API  <a href="#table-of-contents" target="_self">🔙</a></h2>   
