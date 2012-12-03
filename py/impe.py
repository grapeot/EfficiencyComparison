import numpy as np
import time

sift1 = np.random.rand(1000, 128)
sift2 = np.random.rand(1000, 128)

t = time.time()
matched = []
for s1 in sift1:
    dists = []
    for s2 in sift2:
        dists.append(np.linalg.norm(s1 - s2))
    matched.append(np.argmin(dists))

print time.time() - t
