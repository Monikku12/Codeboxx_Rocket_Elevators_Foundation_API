# Rocket_Elevators_Rest_API

System requirements 
If using Windows, "Windows Subsystem for Linux" (WSL) should be installed 
Windows 11: https://www.howtogeek.com/744328/how-to-install-the-windows-subsystem-for-linux-on-windows-11/ 
Windows 10: https://www.omgubuntu.co.uk/how-to-install-wsl2-on-windows-10 
Unbuntu should also be installed for Windows users https://apps.microsoft.com/store/detail/ubuntu-2204-lts/9PN20MSR04DW?hl=en-ca&gl=CA

System dependencies MySQL: https://www.digitalocean.com/community/tutorials/how-to-install-mysql-on-ubuntu-22-04 
PostgreSQL: https://www.digitalocean.com/community/tutorials/how-to-install-and-use-postgresql-on-ubuntu-22-04 

Database creation DBeaver community : https://dbeaver.io/download/

Database initialization 
To initialize the database: 
1- be sure to have your mySQL and postgreSQL server started. 
2- to setup the database, run the command: rails db:setup 
3- to fill your database with fake data and see all the information in the back office, run the command: rails fake:data 
4- From the website, a login panel will get you to the admin section. You will be able to manage your users, your employees and the quotes requested by your clients.

Deployment instructions Go to www.mathieubernier.com to checkout all the functionalities of the website

The REST API allows you to interact with the database on multiple points.

On Postman, a collection of end points are predifine to show you what is possible with the API.
https://www.getpostman.com/collections/f5d96eab48c383dd822b 

Example of a query to find the specific status of a battery

```

  // End point to get a specific status from a specific battery from the list
        // GET: api/Batteries/status/(id#) - Status
        [HttpGet("status/{id}")]
        public async Task<ActionResult<string>> GetBatteryStatus(long id)
        {
          if (_context.batteries == null)
          {
              return NotFound();
          }
            var battery = await _context.batteries.FindAsync(id);
            var status = battery.Status;

            if (battery == null)
            {
                return NotFound();
            }

            return status;
        }


```

Example of a query to find the all the buildings with an intervention status on their equipments

```

        // End point to get all the buildings with a requiring intervention status on their equipments
        // GET: api/Buildings/list - not operational elevators list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildingIntervention(long id)
        {
            if (_context.buildings == null)
            {
                return NotFound();
            }

                var buildinglist = await _context.buildings.ToListAsync();
                var batterylist = await _context.batteries.ToListAsync();
                var columnlist = await _context.columns.ToListAsync();
                var elevatorlist = await _context.elevators.ToListAsync();
                var newbuildinglist = new List<Building>();

            if (_context.buildings != null)
            {
                for (int i = 0; i < buildinglist.Count; i++)
                {
                    if (batterylist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync(batterylist[i].Building_id));
                    }
                }
            
                for (int i = 0; i < batterylist.Count; i++)
                {
                    if (columnlist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync((await _context.batteries.FindAsync(columnlist[i].Battery_id)).Building_id));
                    }

                }
           
                for (int i = 0; i < columnlist.Count; i++)
                {
                    if (elevatorlist[i].Status is "Intervention")
                    {
                        newbuildinglist.Add(await _context.buildings.FindAsync((await _context.batteries.FindAsync((await _context.columns.FindAsync(elevatorlist[i].Column_id)).Battery_id)).Building_id));
                    }
                }
            }

            if (newbuildinglist == null)
            {
                return NotFound();
            }

            return Ok(newbuildinglist.Distinct());
        }

```