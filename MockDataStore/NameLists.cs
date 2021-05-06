namespace MockDataStore
{
    //Originally did as text files, but didn't like the dependency.  My apologies in advance for the long code.
    public class NameLists
    {
        public string[] CommonLastNames { get; set; }
        public string[] FemaleNames { get; set; }
        public string[] MaleNames { get; set; }
        public string[] UncommonLastNames { get; set; }
        public NameLists()
        {
            CommonLastNames = new[]
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson",
                "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore", "Martin", "Jackson", "Thompson",
                "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis", "Robinson", "Walker", "Perez", "Hall",
                "Young", "Allen", "Sanchez", "Wright", "King", "Scott", "Green", "Baker", "Adams", "Nelson", "Hill",
                "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter", "Phillips", "Evans", "Turner", "Torres",
                "Parker", "Collins", "Edwards", "Stewart", "Flores", "Morris", "Nguyen", "Murphy", "Rivera", "Cook",
                "Rogers", "Morgan", "Peterson", "Cooper", "Reed", "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward",
                "Cox", "Diaz", "Richardson", "Wood", "Watson", "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz",
                "Hughes", "Price", "Myers", "Long", "Foster", "Sanders", "Ross", "Morales", "Powell", "Sullivan",
                "Russell", "Ortiz", "Jenkins", "Gutierrez", "Perry", "Butler", "Barnes", "Fisher",
            };

            UncommonLastNames = new[]
            {
                "Afify", "Allaband", "Amspoker", "Ardolf", "Atonal", "Banasiewicz", "Beischel", "Bidelspach",
                "Bombardo", "Bressett", "Bullara", "Calascione", "Carpiniello", "Chaparala", "Chorro", "Clyborne",
                "Concord", "Criddle", "Dallarosa", "Delatejera", "Denetsosie", "Dierksheide", "Dolivo", "Doxon",
                "Duckstein", "Ekundayo", "Eswaran", "Featheringham", "Feyrer", "Floding", "Freling", "Gancayco",
                "Gayhardt", "Gessele", "Ginart", "Goscicki", "Grigoras", "Guillebeaux", "Hanschu", "Hayda", "Henris",
                "Hinsen", "Hoig", "Hulls", "Ionadi", "Javernick", "Jonguitud", "Kasprak", "Kentala", "Kleinhaus",
                "Konietzko", "Kronbach", "Kustka", "Lahde", "Latcha", "Leneghan", "Llama", "Luettgen", "Madris",
                "Maloles", "Marudas", "Mccallops", "Melgren", "Mickelberg", "Mishchuk", "Mosheyev", "Naese", "Nierling",
                "Occhialini", "Ollenburger", "Owsinski", "Panchak", "Pegany", "Petrunich", "Ploense", "Protich",
                "Ragsdill", "Reat", "Riggie", "Rugger", "Salotto", "Scheben", "Schoellman", "Serranogarcia",
                "Shuldberg", "Skalbeck", "Snearl", "Spedoske", "Stawarski", "Stolly", "Suco", "Tahhan", "Tartal",
                "Throndsen", "Torsney", "Tuffin", "Usoro", "Vanidestine", "Viglianco", "Vozenilek",
            };
            FemaleNames = new[]
            {
                "Olivia", "Emma", "Ava", "Sophia", "Isabella", "Charlotte", "Amelia", "Mia", "Harper", "Evelyn",
                "Abigail", "Emily", "Ella", "Elizabeth", "Camila", "Luna", "Sofia", "Avery", "Mila", "Aria", "Scarlett",
                "Penelope", "Layla", "Chloe", "Victoria", "Madison", "Eleanor", "Grace", "Nora", "Riley", "Zoey",
                "Hannah", "Hazel", "Lily", "Ellie", "Violet", "Lillian", "Zoe", "Stella", "Aurora", "Natalie", "Emilia",
                "Everly", "Leah", "Aubrey", "Willow", "Addison", "Lucy", "Audrey", "Bella", "Nova", "Brooklyn",
                "Paisley", "Savannah", "Claire", "Skylar", "Isla", "Genesis", "Naomi", "Elena", "Caroline", "Eliana",
                "Anna", "Maya", "Valentina", "Ruby", "Kennedy", "Ivy", "Ariana", "Aaliyah", "Cora", "Madelyn", "Alice",
                "Kinsley", "Hailey", "Gabriella", "Allison", "Gianna", "Serenity", "Samantha", "Sarah", "Autumn",
                "Quinn", "Eva", "Piper", "Sophie", "Sadie", "Delilah", "Josephine", "Nevaeh", "Adeline", "Arya",
                "Emery", "Lydia", "Clara", "Vivian", "Madeline", "Peyton", "Julia", "Rylee",
            };
            MaleNames = new[]
            {
                "Liam", "Noah", "Oliver", "William", "Elijah", "James", "Benjamin", "Lucas", "Mason", "Ethan",
                "Alexander", "Henry", "Jacob", "Michael", "Daniel", "Logan", "Jackson", "Sebastian", "Jack", "Aiden",
                "Owen", "Samuel", "Matthew", "Joseph", "Levi", "Mateo", "David", "John", "Wyatt", "Carter", "Julian",
                "Luke", "Grayson", "Issac", "Jayden", "Theodore", "Gabriel", "Anthony", "Dylan", "Leo", "Lincoln",
                "Jaxon", "Asher", "Christopher", "Josiah", "Andrew", "Thomas", "Joshua", "Ezra", "Hudson", "Charles",
                "Caleb", "Isaiah", "Ryan", "Nathan", "Adrian", "Christian", "Maverick", "Colton", "Elias", "Aaron",
                "Eli", "Landon", "Jonathan", "Nolan", "Hunter", "Cameron", "Connor", "Santiago", "Jeremiah", "Ezekiel",
                "Angel", "Roman", "Easton", "Miles", "Robert", "Jameson", "Nicholas", "Greyson", "Cooper", "Ian",
                "Carson", "Axel", "Jaxson", "Dominic", "Leonardo", "Luca", "Austin", "Jordan", "Adam", "Xavier", "Jose",
                "Jace", "Everett", "Declan", "Evan", "Kayden", "Parker", "Wesley", "Kai",
            };
        }
        public string[] UsedFullNames { get; set; }
    }
}