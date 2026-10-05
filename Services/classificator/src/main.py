from confluent_kafka import Consumer
import json
from reddis import handle_redis
from calculator import enrich_dict


consumer_config = {
    'bootstrap.servers': "localhost:9092",
     'group.id': 'group-4gfdfedeeedsffddf4',
     'auto.offset.reset': 'earliest'}

consumer = Consumer(consumer_config)
consumer.subscribe(["alerts"])
while True :
    response = consumer.poll(1)

    if (response is None):
        continue

    res_dict = json.loads(response.value().decode('utf-8')) 

    #now check if exists in reddis
    redis_result = handle_redis(res_dict)
    if not redis_result:
        new_dict = enrich_dict(res_dict)

        #now here need to send the msg to rabbit


    
    
