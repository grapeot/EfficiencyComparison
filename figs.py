import numpy as np
import matplotlib.pyplot as pp
import matplotlib as ml

# For the quantitative results figure.
acc1 = np.asarray([2886, 968, 202, 1294, 1564, 11554, 6081, 6078, 114]) / 1000.0;
acc1_str = [str(x) for x in acc1]

def autolabel(rects, name):
# attach some text labels
    for ii,rect in enumerate(rects):
        height = rect.get_height()
        pp.text(rect.get_x()+rect.get_width()/2., height + 0.1, '%s'% (name[ii]), ha='center', va='bottom', fontsize=12)

fig = pp.figure()
ax = fig.add_subplot(1, 1, 1)
p1 = ax.bar(np.asarray(range(1, len(acc1) + 1)), acc1, width=0.7)
autolabel(p1, acc1_str)
ax.set_xticks(np.asarray(range(len(acc1) + 1)) + 0.35)
ax.set_xticklabels(['', 'C# + Accord.NET', 'C#, functional', 'C#, imperative', 'Python + numpy', 'Python, functional', 'Python, imperative', 'MATLAB, imperative', 'MATLAB functional', 'C++'])
pp.xlim([0, len(acc1) + 2])
fig.autofmt_xdate()
pp.ylabel('Running time (s)')
pp.ylim([0, 15])
pp.show()
