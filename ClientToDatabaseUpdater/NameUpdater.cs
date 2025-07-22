using MySql.Data.MySqlClient;
using System.Text;

namespace ClientToDatabaseUpdater
{
    public partial class NameUpdater : Form
    {
        private MySqlConnection? conn = null;

        public NameUpdater()
        {
            InitializeComponent();

            // Set default MySQL connection values
            txtHost.Text = "localhost";
            txtPort.Text = "3306";
            txtUser.Text = "test";
            txtPassword.Text = "test123";
            txtDatabase.Text = "1900";

            btnConnect.Text = "Connect"; // Single connect/disconnect button
            btnConnect.Click += BtnConnect_Click;
            btnFindClient.Click += btnFindClient_Click;
            btnNpcIniToDb.Click += btnNpcIniToDb_Click;
            btnMnstrToDb.Click += btnMnstrToDb_Click;
            btnItemToDb.Click += btnItemToDb_Click;
            btnMgcToDb.Click += btnMgcToDb_Click;
            btnMapToDb.Click += btnMapToDb_Click;
        }

        public class FdbField
        {
            public byte Type;
            public int Offset;
            public string Name { get; set; } = string.Empty; // Ensure 'Name' is initialized
        }

        // --- FdbLoaderEPLStyle ---
        public static class FdbLoaderEPLStyle
        {
            public static (List<FdbField>, List<List<object>>, byte[]) Load(string path)
            {
                var data = File.ReadAllBytes(path);
                const int HEADER_SIZE = 0x20;

                byte[] header = new byte[HEADER_SIZE];
                Array.Copy(data, 0, header, 0, HEADER_SIZE);

                int fieldCount = BitConverter.ToInt32(data, 0x14);
                int rowCount = BitConverter.ToInt32(data, 0x18);
                int textLen = BitConverter.ToInt32(data, 0x1C);
                int textBase = data.Length - textLen;

                var gbk = Encoding.GetEncoding("GBK");
                List<string> labels = new();
                int ptr = textBase;
                for (int i = 0; i < fieldCount; i++)
                {
                    int start = ptr;
                    while (ptr < data.Length && data[ptr] != 0) ptr++;
                    labels.Add(gbk.GetString(data, start, ptr - start));
                    ptr++; // skip null
                }

                var fields = new List<FdbField>();
                for (int i = 0; i < fieldCount; i++)
                {
                    int fieldOffset = HEADER_SIZE + i * 5;
                    byte type = data[fieldOffset];
                    fields.Add(new FdbField
                    {
                        Type = type,
                        Name = labels[i]
                    });
                }

                int ptrTableOffset = HEADER_SIZE + fieldCount * 5;
                var rowPtrs = new List<int>();
                for (int i = 0; i < rowCount; i++)
                {
                    int recPos = ptrTableOffset + i * 8;
                    int recPtr = BitConverter.ToInt32(data, recPos + 4);
                    rowPtrs.Add(recPtr);
                }

                var rows = new List<List<object>>(rowCount);
                foreach (var rowPtr in rowPtrs)
                {
                    if (rowPtr <= 0 || rowPtr == unchecked((int)0xD6000000))
                    {
                        rows.Add(Enumerable.Repeat<object>("", fields.Count).ToList());
                        continue;
                    }
                    int pos = rowPtr;
                    var values = new List<object>(fields.Count);
                    for (int f = 0; f < fields.Count; f++)
                    {
                        var field = fields[f];
                        object val = null;
                        switch (field.Type)
                        {
                            case 1: val = data[pos]; pos += 1; break;
                            case 2: val = BitConverter.ToInt16(data, pos); pos += 2; break;
                            case 3: val = (ushort)BitConverter.ToInt16(data, pos); pos += 2; break;
                            case 4: val = BitConverter.ToInt32(data, pos); pos += 4; break;
                            case 5: val = (uint)BitConverter.ToInt32(data, pos); pos += 4; break;
                            case 6: val = BitConverter.ToSingle(data, pos); pos += 4; break;
                            case 7: val = BitConverter.ToDouble(data, pos); pos += 8; break;
                            case 8: val = BitConverter.ToInt64(data, pos); pos += 8; break;
                            case 9: val = (ulong)BitConverter.ToInt64(data, pos); pos += 8; break;
                            case 10:
                                int strPtr = BitConverter.ToInt32(data, pos);
                                int strAddr = textBase + strPtr;
                                val = "";
                                if (strAddr >= 0 && strAddr < data.Length)
                                {
                                    int end = strAddr;
                                    while (end < data.Length && data[end] != 0) end++;
                                    val = gbk.GetString(data, strAddr, end - strAddr);
                                }
                                pos += 4;
                                break;
                            default:
                                val = ""; break;
                        }
                        values.Add(val);
                    }
                    rows.Add(values);
                }
                return (fields, rows, header);
            }
        }

        // Single button handler for connect/disconnect
        private void BtnConnect_Click(object? sender, EventArgs e)
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                // Try connect
                string connStr = $"server={txtHost.Text};port={txtPort.Text};user={txtUser.Text};password={txtPassword.Text};database={txtDatabase.Text}";
                try
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                    lblStatus.Text = "Status: Connected";
                    btnConnect.Text = "Disconnect";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Status: Error - " + ex.Message;
                }
            }
            else
            {
                // Disconnect
                try
                {
                    conn.Close();
                    lblStatus.Text = "Status: Disconnected";
                    btnConnect.Text = "Connect";
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Status: Error - " + ex.Message;
                }
            }
        }

        private void btnFindClient_Click(object? sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select EO Client folder";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtClientPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnNpcIniToDb_Click(object? sender, EventArgs e)
        {
            // Check connection
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }

            // Search npc.ini in subfolder 'ini'
            string clientPath = txtClientPath.Text.Trim();
            string npcIniPath = Path.Combine(clientPath, "ini", "npc.ini");
            if (!File.Exists(npcIniPath))
            {
                MessageBox.Show("npc.ini not found in client 'ini' folder!");
                return;
            }

            // Parse npc.ini
            var npcDict = new Dictionary<string, string>(); // Key: NpcTypeXXXX, Value: Name/Note
            string? currentSection = null;
            foreach (var line in File.ReadAllLines(npcIniPath))
            {
                var l = line.Trim();
                if (l.StartsWith("[NpcType") && l.EndsWith("]"))
                {
                    currentSection = l.Substring(1, l.Length - 2); // e.g., NpcType315
                }
                else if (!string.IsNullOrEmpty(currentSection) && (l.StartsWith("Name=") || l.StartsWith("Note=")))
                {
                    if (!npcDict.ContainsKey(currentSection) && l.StartsWith("Name="))
                    {
                        npcDict[currentSection] = l.Substring(5);
                    }
                    else if (!npcDict.ContainsKey(currentSection) && l.StartsWith("Note="))
                    {
                        npcDict[currentSection] = l.Substring(5);
                    }
                }
            }

            // Update tables
            string[] tables = { "cq_npc", "cq_dynanpc" };
            foreach (var table in tables)
            {
                string sql = $"SELECT id, lookface, name FROM {table}";
                using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var updateList = new List<(int id, string lookface, string name)>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string lookface = reader["lookface"]?.ToString() ?? string.Empty;
                        if (string.IsNullOrEmpty(lookface)) continue;

                        // Remove last digit
                        string lookfaceKey = lookface.Length > 1 ? lookface.Substring(0, lookface.Length - 1) : lookface;
                        string npcSection = $"NpcType{lookfaceKey}";

                        if (npcDict.TryGetValue(npcSection, out string? npcName) && npcName != null)
                        {
                            // Trim max 24 chars
                            string trimmedName = npcName.Length > 24 ? npcName.Substring(0, 24) : npcName;
                            updateList.Add((id, lookface, trimmedName));
                        }
                    }
                    reader.Close();

                    // Bulk update
                    foreach (var upd in updateList)
                    {
                        string upSql = $"UPDATE {table} SET name=@name WHERE id=@id";
                        using (var upCmd = new MySql.Data.MySqlClient.MySqlCommand(upSql, conn))
                        {
                            upCmd.Parameters.AddWithValue("@name", upd.name);
                            upCmd.Parameters.AddWithValue("@id", upd.id);
                            upCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            MessageBox.Show("Finished updating NPC names to database!");
        }

        private void btnMnstrToDb_Click(object? sender, EventArgs e)
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }

            string clientPath = txtClientPath.Text.Trim();
            string monsterFdbPath = Path.Combine(clientPath, "ini", "monster.fdb");
            if (!File.Exists(monsterFdbPath))
            {
                MessageBox.Show("monster.fdb not found in client 'ini' folder!");
                return;
            }

            // Load FDB using repo style
            List<FdbField> fields;
            List<List<object>> rows;
            byte[] header;
            try
            {
                (fields, rows, header) = FdbLoaderEPLStyle.Load(monsterFdbPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read FDB: {ex.Message}");
                return;
            }

            // Find index for "idType" and "szMonster" fields ONLY
            int idIdx = fields.FindIndex(f => string.Equals(f.Name, "idType", StringComparison.OrdinalIgnoreCase));
            int nameIdx = fields.FindIndex(f => string.Equals(f.Name, "szMonster", StringComparison.OrdinalIgnoreCase));
            if (idIdx == -1 || nameIdx == -1)
            {
                MessageBox.Show("Field 'idType' or 'szMonster' not found in monster.fdb!");
                return;
            }

            var monsters = new List<(int idType, string szMonster)>();
            foreach (var row in rows)
            {
                if (row == null || row.Count <= Math.Max(idIdx, nameIdx))
                    continue;

                int idType = 0;
                var idVal = row[idIdx];
                if (idVal == null || !int.TryParse(idVal.ToString(), out idType))
                    continue;

                string szMonster = row[nameIdx]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(szMonster))
                    continue;
                string trimmedName = szMonster.Length > 24 ? szMonster.Substring(0, 24) : szMonster;

                monsters.Add((idType, trimmedName));
            }

            // UPDATE: use idType and szMonster for table cq_monstertype!
            int updated = 0;
            foreach (var monster in monsters)
            {
                string upSql = "UPDATE cq_monstertype SET name=@szMonster WHERE id=@idType";
                using (var upCmd = new MySql.Data.MySqlClient.MySqlCommand(upSql, conn))
                {
                    upCmd.Parameters.AddWithValue("@szMonster", monster.szMonster);
                    upCmd.Parameters.AddWithValue("@idType", monster.idType);
                    int result = upCmd.ExecuteNonQuery();
                    if (result > 0) updated++;
                }
            }

            MessageBox.Show($"Finished updating Monster names to database!\nTotal updated: {updated}");
        }


        private void btnItemToDb_Click(object? sender, EventArgs e)
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }

            string clientPath = txtClientPath.Text.Trim();
            string itemtypeFdbPath = Path.Combine(clientPath, "ini", "itemtype.fdb");
            if (!File.Exists(itemtypeFdbPath))
            {
                MessageBox.Show("itemtype.fdb not found in client 'ini' folder!");
                return;
            }

            // Load FDB
            List<FdbField> fields;
            List<List<object>> rows;
            byte[] header;
            try
            {
                (fields, rows, header) = FdbLoaderEPLStyle.Load(itemtypeFdbPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read FDB: {ex.Message}");
                return;
            }

            // Find index for uID and szName
            int idIdx = fields.FindIndex(f => string.Equals(f.Name, "uID", StringComparison.OrdinalIgnoreCase));
            int nameIdx = fields.FindIndex(f => string.Equals(f.Name, "szName", StringComparison.OrdinalIgnoreCase));
            if (idIdx == -1 || nameIdx == -1)
            {
                MessageBox.Show("Field 'uID' or 'szName' not found in itemtype.fdb!");
                return;
            }

            var items = new List<(int id, string name)>();
            foreach (var row in rows)
            {
                if (row == null || row.Count <= Math.Max(idIdx, nameIdx))
                    continue;

                int id = 0;
                var idVal = row[idIdx];
                if (idVal == null || !int.TryParse(idVal.ToString(), out id))
                    continue;

                string name = row[nameIdx]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                string trimmedName = name.Length > 32 ? name.Substring(0, 32) : name;

                items.Add((id, trimmedName));
            }

            // UPDATE to cq_itemtype by id and name
            int updated = 0;
            foreach (var item in items)
            {
                string upSql = "UPDATE cq_itemtype SET name=@name WHERE id=@id";
                using (var upCmd = new MySql.Data.MySqlClient.MySqlCommand(upSql, conn))
                {
                    upCmd.Parameters.AddWithValue("@name", item.name);
                    upCmd.Parameters.AddWithValue("@id", item.id);
                    int result = upCmd.ExecuteNonQuery();
                    if (result > 0) updated++;
                }
            }

            MessageBox.Show($"Finished updating Item names to database!\nTotal updated: {updated}");
        }


        private void btnMgcToDb_Click(object? sender, EventArgs e)
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }

            string clientPath = txtClientPath.Text.Trim();
            string magictypeFdbPath = Path.Combine(clientPath, "ini", "magictype.fdb");
            if (!File.Exists(magictypeFdbPath))
            {
                MessageBox.Show("magictype.fdb not found in client 'ini' folder!");
                return;
            }

            List<FdbField> fields;
            List<List<object>> rows;
            byte[] header;
            try
            {
                (fields, rows, header) = FdbLoaderEPLStyle.Load(magictypeFdbPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to read FDB: {ex.Message}");
                return;
            }

            int typeIdx = fields.FindIndex(f => string.Equals(f.Name, "idMagicType", StringComparison.OrdinalIgnoreCase));
            int nameIdx = fields.FindIndex(f => string.Equals(f.Name, "szName", StringComparison.OrdinalIgnoreCase));
            if (typeIdx == -1 || nameIdx == -1)
            {
                MessageBox.Show("Field 'idMagicType' or 'szName' not found in magictype.fdb!");
                return;
            }

            var magics = new List<(int type, string name)>();
            foreach (var row in rows)
            {
                if (row == null || row.Count <= Math.Max(typeIdx, nameIdx))
                    continue;

                int type = 0;
                var typeVal = row[typeIdx];
                if (typeVal == null || !int.TryParse(typeVal.ToString(), out type))
                    continue;

                string name = row[nameIdx]?.ToString() ?? "";
                if (string.IsNullOrWhiteSpace(name))
                    continue;
                string trimmedName = name.Length > 34 ? name.Substring(0, 34) : name;

                magics.Add((type, trimmedName));
            }

            int updated = 0;
            foreach (var mgc in magics)
            {
                string upSql = "UPDATE cq_magictype SET name=@name WHERE type=@type";
                using (var upCmd = new MySql.Data.MySqlClient.MySqlCommand(upSql, conn))
                {
                    upCmd.Parameters.AddWithValue("@name", mgc.name);
                    upCmd.Parameters.AddWithValue("@type", mgc.type);
                    int result = upCmd.ExecuteNonQuery();
                    if (result > 0) updated++;
                }
            }

            MessageBox.Show($"Finished updating Magic names to database!\nTotal updated: {updated}");
        }

        private void btnMapToDb_Click(object? sender, EventArgs e)
        {
            if (conn == null || conn.State != System.Data.ConnectionState.Open)
            {
                MessageBox.Show("Please connect to the database first!");
                return;
            }

            string clientPath = txtClientPath.Text.Trim();
            string gamemapIniPath = Path.Combine(clientPath, "ini", "gamemap.ini");
            if (!File.Exists(gamemapIniPath))
            {
                MessageBox.Show("gamemap.ini not found in client 'ini' folder!");
                return;
            }

            var dictList = new List<(int mapdoc, string name, string describe_text)>();
            int currentMapdoc = 0;
            string name = "";
            string describeText = "";

            foreach (var line in File.ReadAllLines(gamemapIniPath))
            {
                var l = line.Trim();
                if (l.StartsWith("[Map") && l.EndsWith("]"))
                {
                    // Save last entry before next map
                    if (currentMapdoc != 0 && !string.IsNullOrEmpty(name))
                        dictList.Add((currentMapdoc, name, describeText));

                    // Reset for new entry
                    name = "";
                    describeText = "";
                    string mapIdStr = l.Substring(4, l.Length - 5);
                    int.TryParse(mapIdStr, out currentMapdoc);
                }
                else if (l.StartsWith("Name="))
                {
                    name = l.Substring(5).Trim();
                }
                else if (l.StartsWith("File="))
                {
                    string filePath = l.Substring(5).Trim();
                    describeText = Path.GetFileName(filePath);
                }
            }

            // Save last entry
            if (currentMapdoc != 0 && !string.IsNullOrEmpty(name))
                dictList.Add((currentMapdoc, name, describeText));

            // Update to cq_map
            int updated = 0;
            foreach (var entry in dictList)
            {
                // Trim according to spec
                string trimmedName = entry.name.Length > 15 ? entry.name.Substring(0, 15) : entry.name;
                string trimmedDesc = entry.describe_text.Length > 127 ? entry.describe_text.Substring(0, 127) : entry.describe_text;

                string upSql = "UPDATE cq_map SET name=@name, describe_text=@desc WHERE mapdoc=@mapdoc";
                using (var upCmd = new MySql.Data.MySqlClient.MySqlCommand(upSql, conn))
                {
                    upCmd.Parameters.AddWithValue("@name", trimmedName);
                    upCmd.Parameters.AddWithValue("@desc", trimmedDesc);
                    upCmd.Parameters.AddWithValue("@mapdoc", entry.mapdoc);
                    int result = upCmd.ExecuteNonQuery();
                    if (result > 0) updated++;
                }
            }

            MessageBox.Show($"Finished updating gamemap to database!\nTotal updated: {updated}");
        }
    }
}
