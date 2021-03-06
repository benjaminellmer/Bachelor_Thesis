# Service-to-service authentication in a microservice deployment 

## Abstract
The microservice architecture is a currently emerging pattern in software engineering.
Instead of having one huge application, the logic is split into numerous smaller units that fulfill one single purpose.
Therefore function calls within the application migrate to remote calls over the network.
Remote calls between the services have to provide mutual authentication to secure the system from spoofing.
Therefore service-to-service authentication mechanisms are necessary.  

The most popular service-to-service authentication mechanisms are self-signed JSON Web Tokens (JWT) and mutual TLS (mTLS).
This thesis describes the fundamental concepts and discusses the motivations and challenges of both mechanisms.
Furthermore, a project that implements the compared authentication mechanisms is reviewed, and the implementation details are shown.
Developers should be able to choose the correct authentication mechanisms for their projects with the knowledge provided in this thesis.  

The comparisons showed that both mechanisms are very efficient and provide the same level of security.
Therefore none of the mechanisms is superior for all cases.
Self-signed JWTs are the preferred authentication mechanism when nonrepudiation is a requirement, when the application tends to share the user-context, or when the developers require to adapt the authentication mechanism with additional parameters.
When none of these requirements apply, mTLS is the preferred approach since it keeps the system simple.

## Videos
[Presentation Video](https://filebox.fhooecloud.at/index.php/s/dd2rB84fWPjYeJ9)  
[Official Promovideo](https://drive.google.com/file/d/1kwnXZHeul1EyKZvslPQLISg2FJRT6bwO/view?usp=sharing)  
[Project Teaser](https://filebox.fhooecloud.at/index.php/s/DqLwGTbncibgM3n)  

## Contents
- [Bachelor thesis](Thesis/BA_Thesis_Ellmer.pdf)
- [Downloaded references of the thesis](Thesis/references)
- [Bachelor thesis presentation](Presentation/BA_Presentation_Ellmer.pdf)
- [Developed ASP.Net library for service-to-service authentication](Implementation/MutualAuthenticationLibrary)
- [Description how to use the developed library](Implementation/Installationsbeschreibung.pdf)
- [Self-signed JWT example project](Implementation/JWT)
- [mTLS example project](Implementation/mTLS)
- [PRO submission](PRO)
- [Source files of the experiment](Experiment)

## LaTeX sources
- [Source of the thesis](Thesis/thesis-src)
- [Source of the self-made diagrams for the thesis](Thesis/diagrams-src)
- [Source of the presentation](Presentation/src)
- [Source of the PRO abstract](PRO/src-abstract)
- [Source of the PRO installation description](PRO/src-installationsbeschreibung)
- [Source of the PRO requirements specifiaction](PRO/src-requirements-specification)
