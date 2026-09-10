using System.Text.Json;
using HelpDeskTicketTracker;

const string DataFilePath = "tickets.json";

List<Ticket> tickets = LoadTickets(DataFilePath);
bool programRunning = true;

while (programRunning)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("   HELP DESK TICKET TRACKER");
    Console.WriteLine("==============================");
    Console.WriteLine();
    Console.WriteLine("1. Create a ticket");
    Console.WriteLine("2. View all tickets");
    Console.WriteLine("3. Search for a ticket");
    Console.WriteLine("4. Update ticket status");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("Select an option: ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateTicket(tickets, DataFilePath);
            break;

        case "2":
            ViewAllTickets(tickets);
            break;

        case "3":
            SearchForTicket(tickets);
            break;

        case "4":
            UpdateTicketStatus(tickets, DataFilePath);
            break;

        case "0":
            programRunning = false;
            Console.WriteLine("\nGoodbye!");
            break;

        default:
            Console.WriteLine("\nInvalid option. Please try again.");
            PauseProgram();
            break;
    }
}

static void PauseProgram()
{
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

static void CreateTicket(List<Ticket> tickets, string dataFilePath)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("       CREATE NEW TICKET");
    Console.WriteLine("==============================");
    Console.WriteLine();

    string title = ReadRequiredInput("Enter the ticket title: ");
    string description = ReadRequiredInput("Enter the description: ");

    Ticket newTicket = new Ticket
    {
        Id = tickets.Count + 1,
        Title = title,
        Description = description,
        Status = "Open",
        CreatedAt = DateTime.Now
    };

    tickets.Add(newTicket);
    SaveTickets(tickets, dataFilePath);

    Console.WriteLine("\nTicket created successfully!");
    Console.WriteLine($"Ticket ID: {newTicket.Id}");
    Console.WriteLine($"Status: {newTicket.Status}");

    PauseProgram();
}

static void ViewAllTickets(List<Ticket> tickets)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("         ALL TICKETS");
    Console.WriteLine("==============================");
    Console.WriteLine();

    if (tickets.Count == 0)
    {
        Console.WriteLine("No tickets have been created.");
        PauseProgram();
        return;
    }

    foreach (Ticket ticket in tickets)
    {
        Console.WriteLine($"Ticket ID: {ticket.Id}");
        Console.WriteLine($"Title: {ticket.Title}");
        Console.WriteLine($"Description: {ticket.Description}");
        Console.WriteLine($"Status: {ticket.Status}");
        Console.WriteLine(
            $"Created: {ticket.CreatedAt:dd MMM yyyy, hh:mm tt}"
        );
        Console.WriteLine("------------------------------");
    }

    Console.WriteLine($"Total tickets: {tickets.Count}");

    PauseProgram();
}

static void SearchForTicket(List<Ticket> tickets)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("       SEARCH FOR TICKET");
    Console.WriteLine("==============================");
    Console.WriteLine();

    if (tickets.Count == 0)
    {
        Console.WriteLine("There are no tickets to search.");
        PauseProgram();
        return;
    }

    Console.Write("Enter the ticket ID: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int ticketId))
    {
        Console.WriteLine("\nPlease enter a valid number.");
        PauseProgram();
        return;
    }

    Ticket? foundTicket = tickets.Find(
        ticket => ticket.Id == ticketId
    );

    if (foundTicket == null)
    {
        Console.WriteLine($"\nTicket #{ticketId} was not found.");
        PauseProgram();
        return;
    }

    Console.WriteLine("\nTicket found:");
    Console.WriteLine("------------------------------");
    Console.WriteLine($"Ticket ID: {foundTicket.Id}");
    Console.WriteLine($"Title: {foundTicket.Title}");
    Console.WriteLine($"Description: {foundTicket.Description}");
    Console.WriteLine($"Status: {foundTicket.Status}");
    Console.WriteLine(
        $"Created: {foundTicket.CreatedAt:dd MMM yyyy, hh:mm tt}"
    );

    PauseProgram();
}

static void UpdateTicketStatus(
    List<Ticket> tickets,
    string dataFilePath)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("     UPDATE TICKET STATUS");
    Console.WriteLine("==============================");
    Console.WriteLine();

    if (tickets.Count == 0)
    {
        Console.WriteLine("There are no tickets to update.");
        PauseProgram();
        return;
    }

    Console.Write("Enter the ticket ID: ");
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out int ticketId))
    {
        Console.WriteLine("\nInvalid ticket ID.");
        PauseProgram();
        return;
    }

    Ticket? foundTicket =
        tickets.FirstOrDefault(ticket => ticket.Id == ticketId);

    if (foundTicket == null)
    {
        Console.WriteLine("\nTicket not found.");
        PauseProgram();
        return;
    }

    Console.WriteLine($"\nCurrent status: {foundTicket.Status}");
    Console.WriteLine();
    Console.WriteLine("1. Open");
    Console.WriteLine("2. In Progress");
    Console.WriteLine("3. Resolved");
    Console.WriteLine("4. Closed");
    Console.Write("\nSelect the new status: ");

    string? statusChoice = Console.ReadLine();

    string? newStatus = statusChoice switch
    {
        "1" => "Open",
        "2" => "In Progress",
        "3" => "Resolved",
        "4" => "Closed",
        _ => null
    };

    if (newStatus == null)
    {
        Console.WriteLine("\nInvalid status selection.");
        PauseProgram();
        return;
    }

    foundTicket.Status = newStatus;
    SaveTickets(tickets, dataFilePath);

    Console.WriteLine($"\nTicket {foundTicket.Id} updated successfully.");
    Console.WriteLine($"New status: {foundTicket.Status}");

    PauseProgram();
}
static void SaveTickets(List<Ticket> tickets, string dataFilePath)
{
    JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    string json = JsonSerializer.Serialize(tickets, options);

    File.WriteAllText(dataFilePath, json);
}

static List<Ticket> LoadTickets(string dataFilePath)
{
    if (!File.Exists(dataFilePath))
    {
        return new List<Ticket>();
    }

    try
    {
        string json = File.ReadAllText(dataFilePath);

        List<Ticket>? savedTickets =
            JsonSerializer.Deserialize<List<Ticket>>(json);

        return savedTickets ?? new List<Ticket>();
    }
    catch
    {
        Console.WriteLine("The ticket file could not be loaded.");
        return new List<Ticket>();
    }
}











static string ReadRequiredInput(string message)
{
    while (true)
    {
        Console.Write(message);
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
        {
            return input.Trim();
        }

        Console.WriteLine("This field cannot be empty. Please try again.");
    }
}