# Service-to-service authentication in a microservice deployment 

## Expose
In the past years the trend towards extremely scalable architectures, like the microservice architecture emerged in software engineering.
The migration from monolithic architectures towards microservice architectures has enormous consequences regarding the security of software systems.
In monolithic applications, external logic is called using “language-level calls”, which means methods and functions mostly inside the same project.
In microservice applications, external logic is called by using the APIs (Application Programmable Interface) which are provided by external microservices of the same deployment [1].
Of course, this service-to-service communication must be secured properly.
For this problem, there are multiple common approaches that have their own advantages and disadvantages [2].
Usually, in a huge microservice deployment the implementation of the microservices is partitioned into several teams.
In many cases, each team implements the security mechanisms for their microservices on their own, which offers a large attack surface for potential attackers.
This is a tremendous problem because an attacker can exploit the weakness of a single microservice to misuse each other microservice [3].   
  
The target of this bachelor thesis is to compare the most popular approaches for the service-to-service communication in a microservice architecture and point out their advantages and disadvantages.
The common approaches for this problem are trusted networks, mTLS (mutual transport layer security), and JWTs (JSON Web Token) [2]. 
The result of this bachelor thesis shall be an analysis of the different security mechanisms including their use cases and their potential security threats.  
  
### References
[1] Ramaswamy Chandramouli. Microservices-based application systems. NIST
Special Publication, 800:204, 2019.  
  
[2] Wajjakkara Kankanamge Anthony Nuwan Dias and Prabath Siriwardena.
Microservices Security in Action. Simon and Schuster, 2020.  
  
[3] Kai Jander, Lars Braubach, and Alexander Pokahr. Defense-in-depth and
role authentication for microservice systems. Procedia computer science,
130:456–463, 2018
