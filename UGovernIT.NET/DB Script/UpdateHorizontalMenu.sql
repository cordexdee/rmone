UPDATE LandingPages SET Name='Dir. Estimating' WHERE Name ='Dir.Est'
UPDATE LandingPages SET Name='Estimator' WHERE Name ='Estmtr'
UPDATE LandingPages SET Name='Executive' WHERE Name ='Exec'
UPDATE LandingPages SET Name='Marketing' WHERE Name ='Mktg'
UPDATE LandingPages SET Name='Project Manager' WHERE Name ='PM'

UPDATE AspNetRoles SET Name='Dir. Estimating', Title='Dir. Estimating', Discriminator='Dir. Estimating' WHERE Title ='Dir.Est' 
UPDATE AspNetRoles SET Name='Estimator', Title='Estimator', Discriminator='Estimator' WHERE Title ='Estmtr' 
UPDATE AspNetRoles SET Name='Executive', Title='Executive', Discriminator='Executive' WHERE Title ='Exec' 
UPDATE AspNetRoles SET Name='Marketing', Title='Marketing', Discriminator='Marketing' WHERE Title ='Mktg' 
UPDATE AspNetRoles SET Name='Project Manager', Title='Project Manager', Discriminator='' WHERE Title ='PM' 