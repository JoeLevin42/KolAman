from confluent_kafka import Consumer
import json
from reddis import handle_redis
from calculator import enrich_dict
from rabbit_service import send_to_rabbit_queue
from log_service import log_to_elastic

def main():
    consumer_config = {
        'bootstrap.servers': "localhost:9092",
        'group.id': 'group-4defeffeddfeffdffedfeffdrefe8',
        'auto.offset.reset': 'earliest'}

    consumer = Consumer(consumer_config)
    consumer.subscribe(["alerts"])
    while True :
        response = consumer.poll(1)

        if (response is None):
            continue
        res_dict = json.loads(response.value().decode('utf-8')) 
        log_to_elastic("INFO" , "dict received from kafka")    

        #now check if exists in reddis
        redis_result = handle_redis(res_dict)
        if not redis_result:
            new_dict = enrich_dict(res_dict)
            print(new_dict)
            #now here need to send the msg to rabbit
            send_to_rabbit_queue(new_dict)
            log_to_elastic("INFO" , "send the dict to rabbit")


if __name__ == "__main__":
    main()
        
