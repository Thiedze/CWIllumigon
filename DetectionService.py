from threading import Thread

import cv2
import numpy as np


class DetectionService:

    def __init__(self):
        self.window_name = "Image"

        self.stopped = False
        self.image = cv2.imread('test.jpg')
        self.red_img = None
        self.gree_img = None
        self.blue_img = None

        cv2.namedWindow('image')

        # create trackbars for color change
        # cv2.createTrackbar('lowH', self.window_name, 0, 179, self.set_value)
        '''cv2.createTrackbar('highH', self.image, 179, 179, nothing)

        cv2.createTrackbar('lowS', self.image, 0, 255, nothing)
        cv2.createTrackbar('highS', self.image, 255, 255, nothing)

        cv2.createTrackbar('lowV', self.image, 0, 255, nothing)
        cv2.createTrackbar('highV', self.image, 255, 255, nothing)'''

    @staticmethod
    def set_value(value):
        print(value)

    @staticmethod
    def scale(percent, image):
        width = int(image.shape[1] * percent / 100)
        height = int(image.shape[0] * percent / 100)
        return cv2.resize(image, (width, height), interpolation=cv2.INTER_AREA)

    def start_show(self):
        Thread(target=self.show, args=()).start()
        return self

    def stop_show(self):
        self.stopped = True

    def show(self):
        while not self.stopped:
            if self.image is not None:
                cv2.imshow("Image", self.image)
            if self.red_img is not None:
                cv2.imshow("Red", self.red_img)
            if self.gree_img is not None:
                cv2.imshow("Green", self.gree_img)
            if self.blue_img is not None:
                cv2.imshow("Blue", self.blue_img)
            if cv2.waitKey(1) == ord("q"):
                self.stopped = True

            low_h = cv2.getTrackbarPos('lowH', 'image')
            high_h = cv2.getTrackbarPos('highH', 'image')
            low_s = cv2.getTrackbarPos('lowS', 'image')
            high_s = cv2.getTrackbarPos('highS', 'image')
            low_v = cv2.getTrackbarPos('lowV', 'image')
            high_v = cv2.getTrackbarPos('highV', 'image')

            hsv = cv2.cvtColor(self.image, cv2.COLOR_BGR2HSV)

            lower_hsv = np.array([low_h, low_s, low_v])
            higher_hsv = np.array([high_h, high_s, high_v])

            mask = cv2.inRange(hsv, lower_hsv, higher_hsv)

            self.red_img = cv2.bitwise_and(self.image, self.image, mask=mask)

    def get_red_segment(self):
        hsv = cv2.cvtColor(self.image, cv2.COLOR_BGR2HSV)
        light_red = np.array([161, 155, 84])
        dark_red = np.array([179, 255, 255])
        mask = cv2.inRange(hsv, light_red, dark_red)
        self.red_img = cv2.bitwise_and(self.image, self.image, mask=mask)

    def get_blue_segment(self):
        hsv = cv2.cvtColor(self.image, cv2.COLOR_BGR2HSV)
        light_blue = np.array([110, 50, 50])
        dark_blue = np.array([130, 255, 255])
        mask = cv2.inRange(hsv, light_blue, dark_blue)
        self.blue_img = cv2.bitwise_and(self.image, self.image, mask=mask)
