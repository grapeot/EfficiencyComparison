import numpy as np
import time

sift1 = np.random.rand(1000, 128)
sift2 = np.random.rand(1000, 128)

t = time.time()
matched = []
for s1 in sift1:
    dists = np.linalg.norm(np.tile(s1, (1000, 1)) - sift2)
    matched.append(np.argmin(dists))

print time.time() - t
