import pika

def send_to_rabbit_queue(alert_dict):
    connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
    channel = connection.channel()

    channel.queue_declare(queue="alerts", durable=True)

    body_msg = str(alert_dict)
    channel.basic_publish(exchange='',
                      routing_key="alerts",
                      body=body_msg)
    print(f" [x] Sent to rabbit {alert_dict.get("alert_id")}")


