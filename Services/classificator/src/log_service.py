from datetime import datetime, timezone
from elasticsearch import Elasticsearch


client = Elasticsearch("http://localhost:9200")


def log_to_elastic(level: str, message: str):
    log = {
        "timestamp": datetime.now(timezone.utc).isoformat(),
        "level": level,
        "message": message
    }

    client.index(
        index="logs",
        document=log
    )
