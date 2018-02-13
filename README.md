# QHS_PM 
   **Quantitative Health Sciences (QHS) Project Tracking System**

>- Created by Yang Rui, Managed by Jason Delos Reyes & Munirih Taʻafaki

>- All content owned and managed by the Department of Complementary & Integrative Medicine, 
   University of Hawaiʻi John A. Burns School of Medicine.  For questions or concerns, please contact qhs@hawaii.edu.

## Description
A project tracking system that keeps tracks of the projects and activities have been and are being completed within 
the Biostatistics and Bioinformatics Core (as Quantitative Health Sciences) of the John A. Burns School of Medicine's
Department of Complementary & Integrative Medicine.  This system is useful in creating reports that have been tracked
through this system.  Only front-end code without sensitive information has been published into GitHub (i.e., sensitive
information is only stored in a database in a secure location).  Please contact
qhs@hawaii.edu for permission on using the front-end code for personal and/or commercial use.

- Created with Visual Studio 2015.
- .NET version: 4.5.2.
- Built with ASP.NET Web Forms with C#, combined with a MS SQL Server database.

### How to test project into local environment:
1. Download the project from GitHub.
2. Connect the database to the project through connection string in Web.config.
3. (Optional) Connect email notification using SMTP (e.g., Gmail).
4. Build the project in the local setup.
>>- If compilation errors exist, download relevant packages and references.
5. Run the project in the local setup and test features as pertinent.
>>- Publish with profile **PM_DEV** if using local IIS server instead of Visual Studio development environment.
6. **Note:** Do *not* push password and connection string into GitHub as it is a publicly accessible website.

### How to post into intranet (internal site)
1. _DON'T FORGET TO GENERATE BACKUP_.
>>- This will come in handy when you will need to quickly restore the last working instance of the site.
2. Solution Explorer > Right click **Project Management** > Publish.
3. Specify the profile **PM**.
4. Publish to the folder setup for internal site (\\\rs\BiostatPM for JABSOM setup) through File System publish.
>>- If 500 Internal Server Error occurs, contact local IT department for responsible (e.g., JABSOM OIT)

### How to post into internet (external site)
1. _DON'T FORGET TO GENERATE BACKUP_.
2. Solution Explorer > Right click **Project Management** > Publish.
3. Specify the profile **PM - External**.
4. Publish to the folder setup for external site (\\\bqhsportal\BiostatPM for JABSOM setup) through File System publish.
5. Don't worry if "validation of viewstate MAC" error appears.  Simply copy Web.config file from backup to location of live site
   as the settings that are not set through Visual Studio are only found in the actual Web.config file itself.
>>- If 500 Internal Server Error occurs, contact local IT department for responsible (e.g., JABSOM OIT)
