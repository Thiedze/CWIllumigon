import json

import requests as requests


class WledService:

    def __init__(self, ip):
        self.ip = ip

    def init_triangle_for_detection(self, triangle):
        url = "http://" + self.ip + "/json/state"
        data = json.dumps(triangle, default=lambda o: o.__dict__, sort_keys=True, indent=4)
        response = requests.post(url=url, data=data)

        if response.status_code == 200:
            print("init started")

