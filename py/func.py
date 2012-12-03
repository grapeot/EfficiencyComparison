import numpy as np
import time

sift1 = np.random.rand(1000, 128)
sift2 = np.random.rand(1000, 128)

t = time.time()
def match(s1):
    def dist(s2):
        return np.linalg.norm(s1 - s2)
    dists = map(dist, s1)
    return np.argmin(dists)
matches = map(match, sift1)

print time.time() - t
