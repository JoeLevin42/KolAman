# Kol AmanSystem 

this is system is used for getting stream of alert and direct them into the right commands

# Hot to run (Decisions later)


### Stans on the root folder 

```
cd KolAman
```

### first run the simulator

```
cd alert-simulator/run.bat
```

(or just press it)


### first Service

```
cd /Services/NotificationsGate

dotnet run
```

### Second service

```
cd /Services/classificator

python -m venv .venv
.venv\Scripts\activate

pip install -r requirements.txt

cd /src

py /main.py
```


### Third Service 

```
cd /Services/CommandsHeadquarters

dotnet run
```
### Fourth Service 

```
cd /Services/OperationsRoom

dotnet run
```


### Fifth Service (API)

```
cd /Services/DashbordApi

dotnet run
```


# Decisions

## First service : (NotificationGate)

I did all the services with di the main service is the file watcher he uses services kafka
for stream that and elastic to save there the logs

## Second Service : (Calculator)

Im receiving the data from kafka in python check the object is valid simply with
out library im calculations the region by the poligon list and adding this field as the right command
then im sending it to rabbit every command to his own queue.
before im proccessing the data im first checking that this is not duplicate by using redis , for every income
alert the id saved in redis so id this is twice the same id redis will tell us if we already recived this alert before 


## Third Service (CommandsHeadquarters)

This service is receiving the alerts from rabbit with one father consumer (abstract class)
and every command have is own independed class that using the father consumer and sends it into his collection in db
again validation the alert thats them not broken or not valid types

using mongo mostly because this is fast database and easy to insert and take from here by time
also even thought the data schema is pretty much the same the collection are independed and not related
one with the each other 


## Fourth Service (OperationsRoom)

This is service take from the db the alerts and cycleing all the alrerts processing them and search for 
indication to something
every 30 second the service is checking all the alerts and proccessing them giving different time 
for every level of priority 
sending the logs to elastic
and sending indication numebr that this is the count of the commons alerts in more than two regions 
that mybe telling us something happening

