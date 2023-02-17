"""
Gene
"""

import names
import sys
import time
import random
import random_address
import sqlite3

start = time.time()

def str_time_prop(start, end, time_format, prop):
    """Get a time at a proportion of a range of two formatted times.

    start and end should be strings specifying times formatted in the
    given format (strftime-style), giving an interval [start, end].
    prop specifies how a proportion of the interval to be taken after
    start.  The returned time will be in the specified format.
    """

    stime = time.mktime(time.strptime(start, time_format))
    etime = time.mktime(time.strptime(end, time_format))

    ptime = stime + prop * (etime - stime)

    return time.strftime(time_format, time.localtime(ptime))


def random_dob():
    return str_time_prop("2/1/1970", "1/1/2006", '%d/%m/%Y', random.random())

def random_toq():
    return str_time_prop("1/1/2009 12:00:00 AM", "20/02/2023 11:59:59 PM", '%d/%m/%Y %H:%M:%S %p', random.random())

def gen_phone():
    first = str(random.randint(100,999))
    second = str(random.randint(1,888)).zfill(3)

    last = (str(random.randint(1,9998)).zfill(4))
    while last in ['1111','2222','3333','4444','5555','6666','7777','8888']:
        last = (str(random.randint(1,9998)).zfill(4))
        
    return '{}{}{}'.format(first,second, last)

domains = [
    "gmail.com",
    "yahoo.com",
    "hotmail.com",
    "aol.com",
    "msn.com",
    "live.com",
    "unipi.gr"
]

number_of_people = 10
total_samples = 40

full_names = []
emails = []
phones = []
#format dd/MM/yyyy
dobs = []
qts = []
addresses = []
toqs = []

people = []
rqs = []
validrqs = []

with open("requests.txt", 'r', encoding='utf-8') as f:
    validrqs = [line.rstrip() for line in f]

if (len(sys.argv) == 3):
    number_of_people = int(sys.argv[1])
    total_samples = int(sys.argv[2])

for sample in range(number_of_people):
    full_names.append(names.get_full_name())
    phones.append(gen_phone())
    addresses.append(random_address.real_random_address_by_state('CA')['address1'])
    dobs.append(random_dob())
    toqs.append(random_toq())
    qts.append(random.choice(validrqs))


for full_name in full_names:
    firstName = str.lower(str.split(full_name, " ")[0])
    lastName = str.lower(str.split(full_name, " ")[1])
    dot = random.choice(["","."])
    number = str(random.randint(0,9999))
    emails.append(firstName + dot + lastName + number + '@' + random.choice(domains))

for i in range(number_of_people):
    person = [full_names[i], emails[i], phones[i], dobs[i], qts[i], addresses[i], toqs[i]]
    people.append(person)

conn = sqlite3.connect("kep.db")
cursor = conn.cursor()

for i in range(total_samples):
    random.seed()
    p = list(random.choice(people))
    p[6] = random_toq()
    p[4] = random.choice(validrqs)
    if(random.randint(0,30) == 15):
        p[5] = random_address.real_random_address_by_state('CA')['address1']
    if(random.randint(0,20) == 10):
        p[2] = gen_phone()
    rqs.append(p)

cursor.execute("drop table Records")
conn.commit()

cursor.execute("CREATE Table IF NOT EXISTS Records (" +
                "id INTEGER," +
                "FullName VARCHAR(255)," +
                "Email VARCHAR(255)," +
                "Phone VARCHAR(16)," +
                "DOB VARCHAR(16)," +
                "Type VARCHAR(255)," +
                "Address VARCHAR(255)," +
                "Time_Of_Query VARCHAR(255)," +
                "PRIMARY KEY (id AUTOINCREMENT));")
conn.commit()

donerq = 0
for p in rqs:
    donerq += 1
    cursor.execute(f"insert into Records (FullName, Email, Phone, DOB, Type, Address, Time_Of_Query)" + 
                   f"values ('{p[0]}','{p[1]}','{p[2]}','{p[3]}','{p[4]}','{p[5]}','{p[6]}');")
    conn.commit()
    print(f"[{round((donerq/len(rqs) * 100), 2)}%] Inserting into DB...", end='\r')

cursor.close()
conn.close()

print(f"Succesfully generated {len(people)} people and {len(rqs)} requests in {time.time() - start}secs")