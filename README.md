# QuickDesk
The help desk software for small companies which are into customer support. 

<img src="https://github.com/saineshwar/QuickDesk/blob/master/snap/logo.png?raw=true" alt="Banner" title="Banner" style="max-width:100%;">
Project is Developed in ASP.NET MVC with SQL Server Database

### Project Description and working

Support Application is for companies which are into customer support. 
Let’s take one example of a small company which sells a product; now a customer buys some product from an online store then after using for some months, it stops functioning then the customer goes online to find customer support details to communicate.

After calling customer support they will ask you your order id when was this product purchase and entire history suddenly you call gets disconnected, wow now again you will call and tell same details then they will log issues in their internal Application.

After Complaint log they will provide you complain number which you still need to remember again after few days you want to know the status of Application then you will call customer support for track status of a product here you won’t remember when you have called last time.
Now using this Support ticket application how you can over this scenario. 

Now when a customer purchases a product, you can share this application details with it or search on the website of your company, you should share the application URL or link of it. 

Now the first time when a customer visits Support ticket application, He or she needs to register them self with valid mobile no and Email in and some necessary details such that agent can know who are they talking with. 

Next step after Registration is, Users, will get an email for verification on the email address which they have used for Registration.
Now to verify just click on confirm email link in the email then it will redirect to portal and show you verification message after doing verification then the only customer will be allowed to log into Application.

Now to login into Application using username and password which you have applied for Registration. After login into application user can create a ticket. After created a ticket, this ticket will be assigned to agents according to the Category and priority you have chosen. As this ticket is assigned to an agent, there are SLA Policies which agent needs to follow ticket should be replied in certain hours or days as SLA Policies are configured. If the agent does not respond to the ticket, then a notification to the agent is shown before the ticket is going to escalate.

Still, if an agent does not respond then it is escalated to a higher authority which is agent admin (team lead) now agent admin (team lead) has next few days or hours as per SLA Policies are configured.

Now if agent admin (team lead) does not reply to ticket it is finally escalated to configured HOD (Head of the department) he is the final person to respond in Hierarchy.

### There are six roles in this Application
1.	SuperAdmin
2.	User
3.	Admin
4.	Agent
5.	AgentAdmin
6.	Hod

<img src="https://github.com/saineshwar/QuickDesk/blob/master/snap/2020-08-19_18-53-17.png?raw=true" alt="Banner" title="Banner" style="max-width:100%;">

### User Role

<img src="https://github.com/saineshwar/QuickDesk/blob/master/snap/2020-08-19_19-18-33.png?raw=true" alt="Banner" title="Banner" style="max-width:100%;">

### Agent Role

<img src="https://github.com/saineshwar/QuickDesk/blob/master/snap/2020-08-19_19-49-09.png?raw=true" alt="Banner" title="Banner" style="max-width:100%;">

### About Platform Used
Entire Application using Microsoft visual studio 2015 with SQL Server 2019. Frame worked used is ASP.NET MVC 5 and language is C# and Dapper, Entity Framework as ORM and Repository Pattern. Microsoft visual studio 2015 with Update 3.

### Link to download VisualStudio
https://www.visualstudio.com/vs/older-downloads/

### Link to download Microsoft SQL Server 2019
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

### External packages which are used in this Project 
1.	AutoMapper
2.	CaptchaMvc
3.	ELMAH
4.	EPPlus
5.	Unity
6.	Unity.Mvc
7.	EntityFramework
8.	Bootstrap

### Design Templates
https://colorlib.com/polygon/gentelella/

### Encryption Library
1.	SHA512
2.	AES

### Project Documents with Screen

[Project Description Document](https://github.com/saineshwar/QuickDesk/wiki)

[SuperAdmin Document](https://github.com/saineshwar/QuickDesk/wiki/SuperAdmin)








