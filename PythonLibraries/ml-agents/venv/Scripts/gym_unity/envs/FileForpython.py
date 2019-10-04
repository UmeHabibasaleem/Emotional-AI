import numpy as np
a = np.asarray([[1,2,4], [4,3,5]])
b = np.asarray([[1,2,4], [74, 5 , 85]])
J = open("newsahre.csv", 'ab')
np.savetxt(J, a , delimiter= ",")
np.savetxt(J, b , delimiter= ",")