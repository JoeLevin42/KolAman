from elasticsearch import AsyncElasticsearch
import datetime

    
async def log(level , message):
    
    client = AsyncElasticsearch(
        hosts=["https://localhost:9200"])
    doc = {"timestamp" :datetime.datetime.now(), "level" : level ,"message" :message}
   
    await client.index(index="logs", document=doc)
    
    

    
