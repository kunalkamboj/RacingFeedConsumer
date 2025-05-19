Overview of the application
1.	Data Generation
•	Uses Bogus library to generate realistic racing data
•	Creates mock race updates with details like:
•	Race information (ID, location, distance, type)
•	Runner details (name, barrier, price, jockey, trainer)
•	Saves data in XML format
2.	Data Processing
•	Reads generated XML files
•	Processes race updates in batches of 100
•	Converts external race format to internal format
•	Handles cancellation and error scenarios
3.	Message Publishing
•	Uses RabbitMQ for message publishing
•	Implements topic-based exchange
•	Publishes race updates to "racing.feed" exchange
•	Uses "racing.update" as routing key
Setup:
1.	Install docker desktop
2.	Run a RabbitMQ docker container locally 
a.	Execute command: docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
3.	Run the Console application : RacingFeedConsumer.
4.	Login to RabbitMQ locally
a.	URL: http://localhost:15672
b.	UserName: guest
c.	Password: guest
5.	Goto Exchanges to view messages flow 
a.	URL: http://localhost:15672/#/exchanges
 
6.	Live view: http://localhost:15672/#/exchanges/%2F/racing.feed
 
