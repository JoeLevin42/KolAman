import json
from shapely.geometry import Point, Polygon

def get_region_with_geopandas(file_path: str, lon: float, lat: float):

    gdf = gpd.read_file(file_path)

    pt = Point(lon, lat)
 
    matched = gdf[gdf.geometry.contains(pt)]

    if not matched.empty:
        return matched.iloc[0]["region"]
    return "OVERSEAS"



def enrich_dict(result_dict):
    new_dict = result_dict
    lon  = result_dict.get("lon")
    lat  = result_dict.get("lat")
    command_name = get_region_with_geopandas("../regions.geojson", lon, lat)
    new_dict["command_name"] = command_name

    return new_dict