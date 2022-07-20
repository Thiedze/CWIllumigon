import os

from DetectionService import DetectionService
from WledService import WledService
from domain.Segment import Segment
from domain.Triangle import Triangle


def create_test_segment():
    triangle = Triangle()
    triangle.set_bri(2)

    segment = Segment(0, 3, 3)
    segment.col.append([0, 0, 255])
    triangle.seg.append(segment)

    segment = Segment(3, 6, 3)
    segment.col.append([0, 255, 0])
    triangle.seg.append(segment)

    segment = Segment(6, 9, 3)
    segment.col.append([255, 0, 0])
    triangle.seg.append(segment)

    return triangle


if __name__ == "__main__":
    # os.environ["QT_QPA_PLATFORM"] = "wayland"
    wled_service = WledService("10.10.12.145")

    test_triangle = create_test_segment()
    wled_service.init_triangle_for_detection(test_triangle)

    detection_service = DetectionService()
    detection_service.start_show()

    detection_service.get_red_segment()
    detection_service.get_blue_segment()
