import json


class Segment:

    def __init__(self, start, stop, len):
        self.start = start
        self.stop = stop
        self.len = len
        self.on = True
        self.col = []