namespace Multiplayer_Software_Game_Engineering.GameData
{
    public static class Constants
    {

        public const string WHO_WILL_FIGHT      = "Who Will You Fight As?";
        public const string CREATE_LOBBY        = "Create New Lobby";
        public const string JOIN_LOBBY          = "Join Existing Lobby";
        public const string BACK                = "Back";
        public const string GAME_OVER           = "Game Over - You Lose!";
        public const string QUIT_GAME           = "Quit Game";
        public const string NEW_CHARACTER       = "Create New Character";
        public const string LOAD_CHARACTER      = "Load Character";
        public const string SELECT_CHARACTER    = "Select Character";
        public const string MAKE_CHOICE         = "Character Number: ";
        public const string MAIN_MENU           = "Return To Main Menu";
        public const string ACCEPT_CHANGES      = "Accept These Changes";
        public const string DISMISS_CHANGES     = "Dismiss These Changes";
        public const string GAME_SETTINGS       = "Game Settings";
        public const string HIGH_SCORES         = "High Scores";
        public const string SINGLE_PLAYER       = "\nServer is currently offline \nPlaying single player story";

        public const string ERROR_CONNECTION    = "Error: Client could not connect \nEnsure the server is running and the firewall is not blocking port ";
        public const string ERROR_GENERIC       = "An Error Has Occured: ";
        
        public const string INFO_CONTROLS       = "Kill the enemy player \nPress [space] to shoot \nPress [w a s d] to move \n\nPress [Enter] to close textbox";
        public const string INFO_WAITING        = "\n\nWaiting for a player to join room";
        public const string INFO_DAMAGED        = "You have taken damage \nProtect yourself! \n\nPress [Enter] to close textbox";

        public enum Direction
        {
            Down,
            Left,
            Right,
            Up
        }
    }
}