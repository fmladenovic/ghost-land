# 🚀 Upravljanje duhovima

Pakmen je igra u kojoj se agent (žuta loptica) kreće kroz lavirint sa ciljem da sakupi sve koglice (hranu) pre nego što ga duhovi (crvene kapsule) uhvate. 
Cilj predmetnog projekta na ovom predmetu jeste da se implementira deo igre koji se odnosi na upravljanje duhovima, odnosno, napraviti da duhovi jure pakmena.

## Implementacija
Igra je implementirana u *Unity game engine*-u. *Unity* razdvaja scenu od programerskih skripti što olakšava podelu posla u razvoju igara. Kako je skripta ta koja pokreće sve agente na sceni ja ću u ovom delu prezentovati samo kod i njegovu logiku.

#### Postavka
Postoji skripta koja generiše okruženje. Postavljaju se zidovi lavirinta i raspoređuje se hrana sa određenim korakom.

<img src="https://user-images.githubusercontent.com/30222786/106384643-36467f00-63cc-11eb-96ae-fe0ba3574c46.png"  alt="Game Setup"  width="100%">

Hrana je ključni deo ove igre zato što se njena interna vrednost konstantno ažurira sa pomeranjem Pakmena.

#### Ažuriranje interne vrednosti hrane
Kada je hrana pojedena od strane Pakmena ona se sakriva, ali i dalje postoji na sceni. Za tu akciju se brine sledeća skripta:

	public class Food : MonoBehaviour
	{
	    public float value = 1000;
	   
	    void OnTriggerEnter(Collider other)
	    {
	        if (other.CompareTag("Player"))
	        {
	            GetComponent<Renderer>().enabled = false;
	            updateValues(other.transform.transform.gameObject);
	        }
	    }


	    void updateValues( GameObject agent  )
	    {
	        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
	        foreach (GameObject f in food)
	        {
	            Food script = f.GetComponent<Food>();
	            script.value = Manhattan_distance(agent, f);
	        }
	    }

	    float Manhattan_distance(GameObject agent, GameObject food )
	    {
	        float agent_x = agent.transform.position.x;
	        float agent_z = agent.transform.position.z;
	        float food_x = food.transform.position.x;
	        float food_z = food.transform.position.z;
	        return Mathf.Abs(agent_x - food_x) + Mathf.Abs(agent_z - food_z);
	    }
	}

Kada pakmen aktivira okidač koji je zakačen za hranu ona prestane da se renderuje (Unity je jednostavno ne prikazuje na ekranu). 
Nakon toga uzima se sva hrana sa scene i njihove vrednosti se ažuriraju *manhattan* rastojanjem između svake pojedinačne hrane i Pakmena. Na ovaj način se prati pozicija pakmena. Pozicije oko njega će imati manju vrednost, a dalje od njega veću.

<img src="https://user-images.githubusercontent.com/30222786/106384948-e5378a80-63cd-11eb-8a07-2e926179887e.gif"  alt="Value Updating"  width="100%">

 Izabrana je *manhattan* distanca kao metrika rastojanja zato što najbolje modeluje pravougla kretanja (u slučaju skretanja).

<img src="https://user-images.githubusercontent.com/30222786/106385046-76a6fc80-63ce-11eb-88de-6c49eb904d70.gif"  alt="Game Setup"  width="100%">

Može se videti da duh juri pakmena cik-cak kada nisu u liniji.

#### Kretanje duhova
Kretanje duhova se reguliše sledećom skriptom:

	public class GhostMovements : MonoBehaviour
	{

	    public float speed = 25f;
	    public float maxSpeed = 5f;

	    float vision_range = 1f;

	    private Rigidbody _rigidbody;
	    private string _current_direction;

	    // Start is called before the first frame update
	    void Start()
	    {
	        _rigidbody = GetComponent<Rigidbody>();
	        _current_direction = "";
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        List<GameObject> near_food = get_near_food();
	        Vector3 move = get_best_move(near_food);

	        string string_direction = convert_to_string(move);
	        if (string_direction != _current_direction)
	        {
	            _current_direction = string_direction;
	            _rigidbody.velocity = Vector3.zero;
	        }

	        Vector3 velocity = _rigidbody.velocity;
	        if (velocity.magnitude < maxSpeed)
	        {
	            _rigidbody.AddForce(move * speed);
	        }
	    }

	    private List<GameObject> get_near_food()
	    {
	        GameObject[] food = GameObject.FindGameObjectsWithTag("Food");
	        List<GameObject> near_food = new List<GameObject>();
	        float start_x = transform.position.x - vision_range;
	        float end_x = transform.position.x + vision_range;
	        float start_z = transform.position.z - vision_range;
	        float end_z = transform.position.z + vision_range;
	        foreach( GameObject f in food )
	        {
	            if(start_x <= f.transform.position.x && f.transform.position.x <= end_x && start_z <= f.transform.position.z && f.transform.position.z <= end_z)
	            {
	                near_food.Add(f);
	            }
	        }
	        return near_food;
	    }

	    private Vector3 get_best_move(List<GameObject>  near_food)
	    {
	        float value = 1000.0f;
	        Vector3 best = Vector3.zero;
	        foreach( GameObject food in near_food )
	        {
	            Food script = food.GetComponent<Food>();
	            if(value > script.value)
	            {
	                Vector3 to_check = food.transform.position - transform.position;
	                to_check = Vector3.Normalize(to_check);
	                to_check = new Vector3(Mathf.Round(to_check.x), 0, Mathf.Round(to_check.z));
	                value = script.value;
	                best = to_check;
	            }
	        }
	        return best;
	    }

	    private string convert_to_string(Vector3 direction )
	    {
	        if(direction.x == 1 && direction.z == 0)
	        {
	            return "right";
	        }
	        else if (direction.x == -1 && direction.z == 0)
	        {
	            return "left";
	        }
	        else if (direction.x == 0 && direction.z == 1)
	        {
	            return "up";
	        }
	        else if (direction.x == 0 && direction.z == -1)
	        {
	            return "down";
	        }
	        return "";
	    }  
	}
Duh uzima svu hranu koju 'vidi' i od te hrane bira onu koja ima najmanju internu vrednost (jer je ona najbliža Pakmenu koga treba da uhvati). Uzima pravac kretanja ka toj hrani. Vidno polje je ograničeno promenljivom *vision_range*  i ono je taman toliko da ne može da dohvati hranu koja se nalazi sa druge strane zida. 
Konverzija pravce u string je pomoćna funkcija koja pomaže pri proveri da li se duh kreće u istom pravcu ili dolazi do promene pravaca. Ako treba da promeni pravac potrebno je da se duh naglo zaustavi, ako je na primer hrana sa manjom internom vrednošću u suprotnom pravcu od onog u kome se duh kreće. Unity računa i inerciju koja se u pomenutom slučaju suprotstavlja nešem željenom pravcu kretanja pa je potrebno anulirati silu inercije.

### Rezultati

Sa jednim agentom:
<img src="https://user-images.githubusercontent.com/30222786/106385644-9ee42a80-63d1-11eb-987a-ae99bb1f1d68.gif"  alt="Value Updating"  width="100%">

Sa dva agenta:
<img src="https://user-images.githubusercontent.com/30222786/106385645-a0adee00-63d1-11eb-8577-e0ec1ee194f1.gif"  alt="Value Updating"  width="100%">
