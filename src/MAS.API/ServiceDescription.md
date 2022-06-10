<h1 id="services-description">Services Description</h2> 

<h2 id="table-of-contents">Table Of Contents</h2>  
- <a href="#services-description" target="_self">Title</a> <br>
- <a href="#table-of-contents" target="_self">Table Of Contents</a> <br>
- <a href="#weather" target="_self">Weather Forecast</a> <br>
- <a href="#account" target="_self">Account</a> <br>
- <a href="#major" target="_self">Major</a> <br>
- <a href="#subject" target="_self">Subject</a> <br>
- <a href="#subjectMentor" target="_self">Subject of Mentor</a> <br>
- <a href="#user" target="_self">Users</a> <br>
- <a href="#question" target="_self">Question</a> <br>
- <a href="#availableSlot" target="_self">Slot</a> <br>
- <a href="#appointment" target="_self">Appointment</a> <br>
- <a href="#email" target="_self">Email</a> <br>
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
> - ``POST /api/mas/v1/accounts/login-user ``  
>   ***Description***: Login User normal (For test)  
>
> - ``POST /api/mas/v1/accounts/register-user ``  
>   ***Description***: Register User Account (For test)   
>

<h2 id="major">Major  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/majors ``  
>   ***Description***: Get all Majors   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by search string.
>
> - ``POST /api/mas/v1/majors ``  
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
> - ``POST /api/mas/v1/subjects ``  
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
> - ``POST /api/mas/v1/mentor-subjects ``  
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

<h2 id="user">User  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``PUT /api/mas/v1/users/personal ``  
>   ***Description***: Update personal info in profile  
>   ***Role Access***: User  
>
> - ``GET /api/mas/v1/users/personal ``  
>   ***Description***: Get own personal information   
>   ***Role Access***: User  
>
> - ``GET /api/mas/v1/users/{userId} ``  
>   ***Description***: Get a specific user info   
>   ***Role Access***: Admin, User  
>
> - ``GET /api/mas/v1/users/mentors ``  
>   ***Description***: Get all mentors / Search mentors	  
>   ***Role Access***: User  
>   ***Extension***: Paging, Search by Name, filter by Subject	
>
> - ``GET /api/mas/v1/users ``  
>   ***Description***: Get all users  
>   ***Role Access***: Admin  
>   ***Extension***: Paging, Search by Name, filter active account 
>
> - ``PUT /api/mas/v1/users/active/{userId} ``  
>   ***Description***: Active or disable user  
>   ***Role Access***: Admin  
>
> - ``PUT /api/mas/v1/users/mentor-request ``  
>   ***Description***: Send mentor request  
>   ***Role Access***: User  
>
> - ``PUT /api/mas/v1/users/accept-request ``  
>   ***Description***: Accept/deny mentor request  
>   ***Role Access***: Admin  
>
> - ``GET /api/mas/v1/users/own/appointments ``  
>   ***Description***: Get all appointments create by self (student)  
>   ***Role Access***: User (Student)  
>   ***Extension***: Paging, filter by slot, order by new appointment 
>
> - ``GET /api/mas/v1/users/mentor/appointments ``  
>   ***Description***: Get all appointments received (mentor)  
>   ***Role Access***: User (Mentor)  
>   ***Extension***: Paging, filter by slot, order by new appointment 
>
> - ``GET /api/mas/v1/users/{userId}/appointments ``  
>   ***Description***: Get all appointments of a specific user  
>   ***Role Access***: Admin  
>   ***Extension***: Paging, filter by slot, order by new appointment 
>
> - ``GET /api/mas/v1/users/own/appointments/{appointmentId} ``  
>   ***Description***: Get a appointment create by self (student)  
>   ***Role Access***: User (Student)  
>
> - ``GET /api/mas/v1/users/mentor/appointments/{appointmentId} ``  
>   ***Description***: Get a appointment received (mentor)  
>   ***Role Access***: User (Mentor)  
>
<h2 id="question">Question  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/questions/{appointmentId} ``  
>   ***Description***: Get all Questions of specific appointment   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging 
>
> - ``POST /api/mas/v1/questions ``  
>   ***Description***: Create a Question   
>   ***Role Access***: User  
>
> - ``GET /api/mas/v1/questions/{questionId} ``  
>   ***Description***: Get Question by Id   
>   ***Role Access***: Admin, User  
>
> - ``PUT /api/mas/v1/questions/{questionId} ``  
>   ***Description***: Answer Question   
>   ***Role Access***: User  
>
> - ``DELETE /api/mas/v1/questions/{questionId} ``  
>   ***Description***: Delete Question by Id   
>   ***Role Access***: User  

<h2 id="availableSlot">Slot  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/slots ``  
>   ***Description***: Get all available slots   
>   ***Role Access***: Admin, User  
>   ***Extension***: Paging, Filter by Mentor Id, Filter by time range, Sorting
>
> - ``POST /api/mas/v1/slots ``  
>   ***Description***: Create a Slot   
>   ***Role Access***: User  
>
> - ``GET /api/mas/v1/slots/{slotId} ``  
>   ***Description***: Get Slot by Id   
>   ***Role Access***: Admin, User  
>
> - ``DELETE /api/mas/v1/slots/{slotId} ``  
>   ***Description***: Delete Slot by Id   
>   ***Role Access***: User   

<h2 id="appointment">Appointment  <a href="#table-of-contents" target="_self">🔙</a></h2>   

> - ``GET /api/mas/v1/appointments/{appointmentId} ``  
>   ***Description***: Get a appointment in detail   
>   ***Role Access***: Admin  
>
> - ``DELETE /api/mas/v1/appointments/{appointmentId} ``  
>   ***Description***: Delete a appointment in detail   
>   ***Role Access***: User  
>
> - ``POST /api/mas/v1/appointments ``  
>   ***Description***: Create a appointment   
>   ***Role Access***: User  
>
> - ``PUT /api/mas/v1/appointments/process/{appointmentId} ``  
>   ***Description***: Mentor process an appointment, decide to approve or deny.   
>   ***Role Access***: User (Mentor)  
>
> - ``PUT /api/mas/v1/appointments/update/{appointmentId} ``  
>   ***Description***: Mentor add time start, time finish, and description of the appointment    
>   ***Role Access***: User (Mentor)  

<h2 id="email">Email  <a href="#table-of-contents" target="_self">🔙</a></h2>  

> - ``POST /api/mas/v1/email/send ``  
>   ***Description***: Send email to a specific user    
>   ***Role Access***: Admin, User        
>
<h2 id="api">MAS API  <a href="#table-of-contents" target="_self">🔙</a></h2>   
