import pika

def send_to_rabbit_queue(alert_dict):
    connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
    channel = connection.channel()

    channel.queue_declare(queue="alerts.north", durable=True)
    channel.queue_declare(queue="alerts.south", durable=True)
    channel.queue_declare(queue="alerts.center", durable=True)
    channel.queue_declare(queue="alerts.overseas", durable=True)

    queue = ""
    if (alert_dict.get("command_name")) == "NORTH":
        queue = "alerts.north"
    elif (alert_dict.get("command_name")) == "SOUTH":
         queue = "alerts.south"
    elif (alert_dict.get("command_name")) == "CENTER":
         queue = "alerts.center"
    elif (alert_dict.get("command_name")) == "OVERSEAS":
         queue = "alerts.overseas"

    body_msg = str(alert_dict)
    channel.basic_publish(exchange='',
                      routing_key=queue,
                      body=body_msg)
    print(f" [x] Sent to rabbit {alert_dict.get("alert_id")}")


