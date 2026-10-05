import redis

def handle_redis(alert : dict)-> bool:
    red = redis.Redis(host='localhost', port=6379, decode_responses=True)
    alert_id = alert.get("alert_id")
    alert_key = f"key:{alert.get(alert_id)}"

    res = red.exists(alert_key)
    if res is None:
        red.set(alert_key,ex=3600)
        return True

    return False


