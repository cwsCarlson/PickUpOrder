# PickUpOrder
An order management system for restaurants that offer pick-up. This was built in ASP.NET MVC for my Processes of Object-Oriented Software class (COP 4331C).

# Building
Use Visual Studio on the project. You will need the "ASP.NET and web development" workload installed, though Visual Studio will download this automatically if you don't already have it.

# Features
* Basic accounts with assigned permissions
* All accounts can make and place orders.
* Employees and managers can view all orders and change their status (Received, Preparing, Done, or Picked Up).
* Managers can add, edit, and remove items and categories.

# Other things to note
* This will run at the URL `https://localhost:44302/`
* Employees are registered at `https://localhost:44302/Registration/SecretRegistration`.
* Managers are registered at `https://localhost:44302/Registration/SuperSecretRegistration`.
