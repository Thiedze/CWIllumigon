class Triangle:

    def __init__(self):
        self.bri = 50
        self.seg = []

    def set_bri(self, percent):
        self.bri = int((255 / 100) * percent)
