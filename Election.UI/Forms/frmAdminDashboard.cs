#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Election.UI.Forms
{
    public partial class FrmAdminDashboard : Form
    {
        #region Fields and Properties
        private readonly HttpClient _client;
        private Panel? _selectedPanel = null;

        // Dashboard statistics labels - 7 CARDS
        private readonly Label[] _statValueLabels = new Label[7];
        private readonly Panel[] _statCards = new Panel[7];

        // Session management
        private System.Windows.Forms.Timer? _sessionTimer;
        private int _inactiveMinutes;
        private bool _isResizing;
        private Size _previousSize;

        // Color scheme
        private static readonly Color COLOR_PRIMARY = Color.FromArgb(15, 22, 40);
        private static readonly Color COLOR_SECONDARY = Color.FromArgb(21, 101, 192);
        private static readonly Color COLOR_SUCCESS = Color.FromArgb(46, 125, 50);
        private static readonly Color COLOR_WARNING = Color.FromArgb(245, 124, 0);
        private static readonly Color COLOR_DANGER = Color.FromArgb(244, 67, 54);
        private static readonly string[] _userRoles = ["All Roles", "Voter", "Candidate", "Admin"];
        private static readonly string[] _userStatus = ["All Status", "Active", "Disabled"];

        private static readonly Color COLOR_INFO = Color.FromArgb(123, 31, 162);
        private static readonly Color COLOR_LIGHT = Color.FromArgb(245, 247, 250);
        private static readonly Color COLOR_DARK = Color.FromArgb(33, 43, 53);

        private static readonly string[] CardTitles =
        [
            "👥 Total Users", "🧑 Total Voters", "🎯 Total Candidates",
            "🗳️ Total Votes", "✅ Approved", "⏳ Pending", "❌ Rejected"
        ];

        private static readonly Color[] CardColors =
        [
            Color.FromArgb(123, 31, 162),  // INFO
            Color.FromArgb(21, 101, 192),  // SECONDARY
            Color.FromArgb(46, 125, 50),   // SUCCESS
            Color.FromArgb(245, 124, 0),    // WARNING
            Color.FromArgb(0, 150, 136),   // Teal
            Color.FromArgb(255, 193, 7),    // Amber
            Color.FromArgb(198, 40, 40)    // DANGER
        ];
        #endregion

        #region Constructor
        public FrmAdminDashboard()
        {
            InitializeComponent();

            // Initialize
            _previousSize = Size;
            MinimumSize = new(1000, 700);

            // Setup HttpClient
            _client = new()
            {
                BaseAddress = new("https://localhost:7208"),
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Add authorization header
            if (!string.IsNullOrEmpty(UserSession.Token))
            {
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", UserSession.Token);
            }

            // Connect event handlers
            ConnectEventHandlers();

            // Setup session timer
            InitializeSessionTimer();

            // Set default view
            ShowDashboardView();
        }
        #endregion

        #region Initialization Methods
        private void ConnectEventHandlers()
        {
            // Form events
            Load += FrmAdminDashboard_Load;
            FormClosing += FrmAdminDashboard_FormClosing;
            Resize += FrmAdminDashboard_Resize;
            ResizeBegin += FrmAdminDashboard_ResizeBegin;
            ResizeEnd += FrmAdminDashboard_ResizeEnd;

            // Mouse/keyboard activity resets timer
            MouseMove += (s, e) => ResetSessionTimer();
            KeyPress += (s, e) => ResetSessionTimer();

            // Header buttons
            btnLogout.Click += BtnLogout_Click;
            btnRefresh.Click += BtnRefresh_Click;

            // Sidebar navigation
            MakePanelClickable(panelDashboard, PanelDashboard_Click);
            MakePanelClickable(panelCandidate, PanelCandidate_Click);
            MakePanelClickable(panelUserManagement, PanelUserManagement_Click);
            MakePanelClickable(panelVoteMonitoring, PanelVoteMonitoring_Click);
            MakePanelClickable(panelElectionControl, PanelElectionControl_Click);
            MakePanelClickable(panelResults, PanelResults_Click);
        }

        private void InitializeSessionTimer()
        {
            _sessionTimer = new System.Windows.Forms.Timer { Interval = 60000 }; // 1 minute
            _sessionTimer.Tick += (s, e) =>
            {
                _inactiveMinutes++;
                if (_inactiveMinutes >= 15) // 15 minutes inactivity
                {
                    _sessionTimer.Stop();
                    MessageBox.Show("Session expired due to inactivity. Please login again.",
                        "Auto Logout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Logout();
                }
            };
            _sessionTimer.Start();
        }

        private void ResetSessionTimer() => _inactiveMinutes = 0;
        #endregion

        #region Responsive Layout Methods
        private void FrmAdminDashboard_ResizeBegin(object? sender, EventArgs e) => _isResizing = true;

        private void FrmAdminDashboard_ResizeEnd(object? sender, EventArgs e)
        {
            _isResizing = false;
            AdjustLayoutForSize();
        }

        private void FrmAdminDashboard_Resize(object? sender, EventArgs e)
        {
            if (!_isResizing) return;
            AdjustLayoutForSize();
            _previousSize = Size;
        }

        private void AdjustLayoutForSize()
        {
            try
            {
                pnlMainContent.SuspendLayout();

                // Calculate positions
                int sidebarWidth = panel1.Width;
                int headerHeight = pnlHeader.Height;
                int adminLabelHeight = 40;

                // Update main content panel
                pnlMainContent.Location = new(sidebarWidth, headerHeight + adminLabelHeight);
                pnlMainContent.Size = new(
                    ClientSize.Width - sidebarWidth,
                    ClientSize.Height - headerHeight - adminLabelHeight
                );

                // Center admin label
                labeladmin.Left = (ClientSize.Width - labeladmin.Width) / 2;
                labeladmin.Top = headerHeight + 5;

                // Update sidebar height
                panel1.Height = ClientSize.Height - pnlHeader.Height;

                // Update election status position
                lblElectionStatusHeader.Left = ClientSize.Width - 300;

                pnlMainContent.ResumeLayout();

                // Adjust dashboard statistics if visible
                if (_statCards[0] != null && _statCards[0].Visible)
                {
                    AdjustDashboardLayout();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Layout adjustment error: {ex.Message}");
            }
        }

        private void AdjustDashboardLayout()
        {
            try
            {
                // Find stats container (now a TableLayoutPanel)
                var statsContainer = pnlMainContent.Controls.OfType<Panel>()
                    .SelectMany(p => p.Controls.OfType<TableLayoutPanel>())
                    .FirstOrDefault();

                if (statsContainer != null)
                {
                    statsContainer.Width = pnlMainContent.Width - 60;
                }
            }
            catch { /* Ignore layout errors */ }
        }
        #endregion

        #region Navigation Methods
        private static void MakePanelClickable(Panel panel, EventHandler clickHandler)
        {
            panel.Cursor = Cursors.Hand;
            panel.Click += clickHandler;
            foreach (Control control in panel.Controls)
            {
                control.Cursor = Cursors.Hand;
                control.Click += clickHandler;
            }
        }

        private void HighlightPanel(Panel panel)
        {
            if (_selectedPanel != null && _selectedPanel != panel)
                ResetPanelAppearance(_selectedPanel);

            if (panel != null)
            {
                SetActivePanelAppearance(panel);
                _selectedPanel = panel;
            }
        }

        private static void ResetPanelAppearance(Panel panel)
        {
            panel.BackColor = COLOR_PRIMARY;
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = COLOR_PRIMARY;
                    label.ForeColor = Color.White;
                }
                else if (control is PictureBox pictureBox)
                    pictureBox.BackColor = COLOR_PRIMARY;
            }
        }

        private static void SetActivePanelAppearance(Panel panel)
        {
            panel.BackColor = COLOR_SECONDARY;
            foreach (Control control in panel.Controls)
            {
                if (control is Label label)
                {
                    label.BackColor = COLOR_SECONDARY;
                    label.ForeColor = Color.LightSkyBlue;
                }
                else if (control is PictureBox pictureBox)
                    pictureBox.BackColor = COLOR_SECONDARY;
            }
        }

        private void FrmAdminDashboard_Load(object? sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                lblSystemTitle.Text = $"ETH Election System | Welcome, {UserSession.Username}";
                labeladmin.Text = $"Welcome Administrator: {UserSession.Username}";
            }

            HighlightPanel(panelDashboard);
            AdjustLayoutForSize();

            // Load initial election status
            _ = UpdateElectionStatusHeaderAsync();
        }

        private void FrmAdminDashboard_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _sessionTimer?.Stop();
            _client?.Dispose();
        }
        #endregion

        #region Navigation Events
        private void PanelDashboard_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelDashboard);
            ShowDashboardView();
        }

        private void PanelCandidate_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelCandidate);
            ShowCandidateManagementView();
        }

        private void PanelUserManagement_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelUserManagement);
            ShowUserManagementView();
        }

        private void PanelVoteMonitoring_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelVoteMonitoring);
            ShowVoteMonitoringView();
        }

        private void PanelElectionControl_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelElectionControl);
            ShowElectionControlView();
        }

        private void PanelResults_Click(object? sender, EventArgs e)
        {
            HighlightPanel(panelResults);
            ShowResultsView();
        }

        private void PanelLogout_Click(object? sender, EventArgs e) => BtnLogout_Click(sender, e);

        private void BtnRefresh_Click(object? sender, EventArgs e) => RefreshCurrentView();

        private async Task UpdateElectionStatusHeaderAsync()
        {
            try
            {
                var response = await _client.GetAsync("api/admin/election-status");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    bool isActive = result.GetProperty("isActive").GetBoolean();
                    bool votingOpen = result.GetProperty("votingOpen").GetBoolean();

                    lblElectionStatusHeader.Text = isActive
                        ? (votingOpen ? "✅ Election Active" : "⏸ Election Paused")
                        : "⏸ Election Inactive";
                    lblElectionStatusHeader.ForeColor = isActive ? COLOR_SUCCESS : COLOR_WARNING;
                }
            }
            catch
            {
                lblElectionStatusHeader.Text = "❌ Status Error";
                lblElectionStatusHeader.ForeColor = COLOR_DANGER;
            }
        }
        #endregion

        #region View Management
        private void ShowDashboardView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "📊 DASHBOARD OVERVIEW";
            CreateDashboardStatistics();
            AdjustLayoutForSize();
        }

        private void ShowCandidateManagementView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "🎯 CANDIDATE MANAGEMENT";
            CreateCandidateManagementPanel();
            AdjustLayoutForSize();
        }

        private void ShowUserManagementView()
        {
            ClearMainContent();
            lblCandidatesTitle.Visible = false; // Hide header to save space
            lblCandidatesTitle.Text = "🧑 USER MANAGEMENT";
            CreateUserManagementPanel();
            AdjustLayoutForSize();
        }

        private void ShowVoteMonitoringView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "🗳️ VOTE MONITORING";
            CreateVoteMonitoringPanel();
            AdjustLayoutForSize();
        }

        private void ShowElectionControlView()
        {
            ClearMainContent();
            lblCandidatesTitle.Text = "⚙️ ELECTION CONTROL";
            CreateElectionControlPanel();
            AdjustLayoutForSize();
        }

        private void ShowResultsView()
        {
            ClearMainContent();
            lblCandidatesTitle.Visible = false; // Hide header to avoid duplicate title
            lblCandidatesTitle.Text = "📊 ELECTION RESULTS";
            CreateResultsPanel();
            AdjustLayoutForSize();
        }

        private void ClearMainContent()
        {
            var controlsToRemove = pnlMainContent.Controls.Cast<Control>()
                .Where(c => c != lblCandidatesTitle && c != btnRefresh && c != dgvCandidates)
                .ToList();

            foreach (var control in controlsToRemove)
                control.Dispose();

            lblCandidatesTitle.Visible = true;
            btnRefresh.Visible = true;
            dgvCandidates.Visible = false;
        }

        private void RefreshCurrentView()
        {
            if (_selectedPanel == panelDashboard) ShowDashboardView();
            else if (_selectedPanel == panelCandidate) ShowCandidateManagementView();
            else if (_selectedPanel == panelUserManagement) ShowUserManagementView();
            else if (_selectedPanel == panelVoteMonitoring) ShowVoteMonitoringView();
            else if (_selectedPanel == panelElectionControl) ShowElectionControlView();
            else if (_selectedPanel == panelResults) ShowResultsView();
        }
        #endregion

        #region Dashboard Statistics - FIXED: Shows actual numbers
        private void CreateDashboardStatistics()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = COLOR_LIGHT,
                Padding = new(20)
            };

            TableLayoutPanel statsContainer = new()
            {
                Location = new(20, 80),
                Size = new(mainPanel.Width - 60, 140),
                ColumnCount = 7,
                RowCount = 1,
                Dock = DockStyle.Top,
                Margin = new(0, 80, 0, 0)
            };

            for (int i = 0; i < 7; i++)
                statsContainer.ColumnStyles.Add(new(SizeType.Percent, 14.28f));

            statsContainer.RowStyles.Add(new(SizeType.Absolute, 130));

            for (int i = 0; i < 7; i++)
            {
                Panel card = CreateStatCard(CardTitles[i], "0", CardColors[i], i);
                card.Dock = DockStyle.Fill;
                statsContainer.Controls.Add(card, i, 0);
                _statCards[i] = card;
            }

            Label lblLoading = new()
            {
                Text = "Loading statistics...",
                Font = new("Segoe UI", 12),
                Location = new(20, 300),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            mainPanel.Controls.Add(statsContainer);
            mainPanel.Controls.Add(lblLoading);
            pnlMainContent.Controls.Add(mainPanel);

            _ = LoadDashboardStatisticsAsync(lblLoading);
        }

        private Panel CreateStatCard(string title, string value, Color color, int index)
        {
            Panel card = new()
            {
                Height = 120,
                Margin = new(8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitle = new()
            {
                Text = title,
                Font = new("Segoe UI", 10, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkGray,
                Padding = new(0, 5, 0, 0)
            };

            _statValueLabels[index] = new()
            {
                Text = value,
                Font = new("Segoe UI", 24, FontStyle.Bold),
                Dock = DockStyle.Fill,
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel bottomBorder = new()
            {
                Size = new(card.Width, 5),
                Location = new(0, card.Height - 5),
                BackColor = color
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(_statValueLabels[index]);
            card.Controls.Add(bottomBorder);

            return card;
        }

        private async Task LoadDashboardStatisticsAsync(Label lblLoading)
        {
            try
            {
                // Get dashboard statistics from API - FIXED PARSING
                var response = await _client.GetAsync("api/admin/dashboard-statistics");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                    // Check if response has success property
                    if (result.TryGetProperty("success", out var success) && success.GetBoolean())
                    {
                        // Check if statistics property exists
                        if (result.TryGetProperty("statistics", out var stats))
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                try
                                {
                                    // Get EXACT properties - these names MUST match AdminController
                                    int totalUsers = GetIntFromJson(stats, "TotalUsers");
                                    int totalVoters = GetIntFromJson(stats, "TotalVoters");
                                    int totalCandidates = GetIntFromJson(stats, "TotalCandidates");
                                    int totalVotes = GetIntFromJson(stats, "TotalVotes");
                                    int approvedCandidates = GetIntFromJson(stats, "ApprovedCandidates");
                                    int pendingCandidates = GetIntFromJson(stats, "PendingCandidates");
                                    int rejectedCandidates = GetIntFromJson(stats, "RejectedCandidates");

                                    // Update ALL 7 cards with actual numbers
                                    _statValueLabels[0].Text = totalUsers.ToString("N0");
                                    _statValueLabels[1].Text = totalVoters.ToString("N0");
                                    _statValueLabels[2].Text = totalCandidates.ToString("N0");
                                    _statValueLabels[3].Text = totalVotes.ToString("N0");
                                    _statValueLabels[4].Text = approvedCandidates.ToString("N0");
                                    _statValueLabels[5].Text = pendingCandidates.ToString("N0");
                                    _statValueLabels[6].Text = rejectedCandidates.ToString("N0");

                                    lblLoading.Visible = false;

                                    // Show success message
                                    lblLoading.Text = "✅ Statistics loaded successfully!";
                                    lblLoading.ForeColor = COLOR_SUCCESS;
                                }
                                catch (Exception ex)
                                {
                                    lblLoading.Text = $"JSON Parse Error: {ex.Message}";
                                    lblLoading.ForeColor = COLOR_DANGER;
                                }
                            });
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                lblLoading.Text = "❌ No 'statistics' property in response";
                                lblLoading.ForeColor = COLOR_DANGER;
                            });
                        }
                    }
                    else
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            lblLoading.Text = "❌ API returned success: false";
                            lblLoading.ForeColor = COLOR_DANGER;
                        });
                    }
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        lblLoading.Text = $"❌ HTTP Error: {response.StatusCode}";
                        lblLoading.ForeColor = COLOR_DANGER;
                    });
                }
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    lblLoading.Text = $"❌ Connection error: {ex.Message}";
                    lblLoading.ForeColor = COLOR_DANGER;
                });
            }
        }

        private static int GetIntFromJson(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
                return ParseJsonInt(prop);

            string camelCaseName = char.ToLower(propertyName[0]) + propertyName[1..];
            if (element.TryGetProperty(camelCaseName, out prop))
                return ParseJsonInt(prop);

            return 0;
        }

        private static int ParseJsonInt(JsonElement prop)
        {
            if (prop.ValueKind == JsonValueKind.Number)
                return prop.GetInt32();
            else if (prop.ValueKind == JsonValueKind.String && int.TryParse(prop.GetString(), out int value))
                return value;
            return 0;
        }
        #endregion

        #region Candidate Management - UPDATED TO USE CandidateController
        private void CreateCandidateManagementPanel()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new(20, 15, 20, 15)
            };

            TableLayoutPanel filterPanel = new()
            {
                Height = 85,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new(10, 5, 10, 5),
                RowCount = 1
            };
            filterPanel.RowStyles.Add(new(SizeType.Percent, 100));
            Label lblSearch = new()
            {
                Text = "Search Name:",
                Font = new("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            TextBox txtSearch = new()
            {
                Font = new("Segoe UI", 10),
                Dock = DockStyle.Fill,
                Margin = new(0, 5, 0, 0)
            };

            filterPanel.ColumnCount = 5;
            filterPanel.ColumnStyles.Clear();
            filterPanel.ColumnStyles.Add(new(SizeType.Absolute, 120));
            filterPanel.ColumnStyles.Add(new(SizeType.Absolute, 150));
            filterPanel.ColumnStyles.Add(new(SizeType.Absolute, 120));
            filterPanel.ColumnStyles.Add(new(SizeType.Absolute, 200));
            filterPanel.ColumnStyles.Add(new(SizeType.Percent, 100));

            Label lblFilter = new()
            {
                Text = "Filter by Status:",
                Font = new("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };

            ComboBox cmbStatus = new()
            {
                Items = { "All Candidates", "Pending", "Approved", "Rejected" },
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new("Segoe UI", 10),
                Dock = DockStyle.Fill,
                Margin = new(0, 10, 0, 0)
            };
            cmbStatus.SelectedIndex = 0;

            Panel spacerPanel = new() { Dock = DockStyle.Fill };

            filterPanel.Controls.Clear();
            filterPanel.Controls.Add(lblFilter, 0, 0);
            filterPanel.Controls.Add(cmbStatus, 1, 0);
            filterPanel.Controls.Add(lblSearch, 2, 0);
            filterPanel.Controls.Add(txtSearch, 3, 0);
            filterPanel.Controls.Add(spacerPanel, 4, 0);

            Panel dgvContainer = new()
            {
                Dock = DockStyle.Fill,
                Padding = new(0, 10, 0, 0)
            };

            DataGridView dgv = new()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(240, 240, 240),
                RowTemplate = { Height = 45 },
                ColumnHeadersHeight = 45,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            dgv.ColumnHeadersDefaultCellStyle = new()
            {
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                Font = new("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new(0, 10, 0, 10)
            };
            dgv.EnableHeadersVisualStyles = false;

            // Columns
            dgv.Columns.Add("FullName", "👤 Candidate Name");
            dgv.Columns.Add("Email", "📧 Email Address");
            dgv.Columns.Add("Party", "🏛️ Political Party");
            dgv.Columns.Add("Status", "📊 Status");
            dgv.Columns.Add("ApplicationDate", "📅 Application Date");

            // Set column widths
            dgv.Columns["FullName"].Width = 140;
            dgv.Columns["Email"].Width = 160;
            dgv.Columns["Party"].Width = 100;
            dgv.Columns["Status"].Width = 80;
            dgv.Columns["ApplicationDate"].Width = 100;

            // Action buttons
            DataGridViewButtonColumn btnView = new()
            {
                Name = "btnView",
                HeaderText = "🔍 View",
                Text = "👁️ View",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };

            DataGridViewButtonColumn btnApprove = new()
            {
                Name = "btnApprove",
                HeaderText = "✅ Appr.",
                Text = "✅ Appr.",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };

            DataGridViewButtonColumn btnReject = new()
            {
                Name = "btnReject",
                HeaderText = "❌ Rej.",
                Text = "❌ Rej.",
                UseColumnTextForButtonValue = true,
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };

            dgv.Columns.Add(btnView);
            dgv.Columns.Add(btnApprove);
            dgv.Columns.Add(btnReject);

            // Button click handlers
            dgv.CellClick += async (s, e) =>
            {
                if (e.RowIndex < 0 || s is not DataGridView dgvSender) return;

                var columns = dgvSender.Columns;

                int viewIndex = columns["btnView"]?.Index ?? -1;
                int approveIndex = columns["btnApprove"]?.Index ?? -1;
                int rejectIndex = columns["btnReject"]?.Index ?? -1;

                if (e.ColumnIndex == viewIndex)
                {
                    if (dgvSender.Rows[e.RowIndex].Tag != null)
                    {
                        int candidateId = Convert.ToInt32(dgvSender.Rows[e.RowIndex].Tag);
                        await ShowCandidateDetailsAsync(_client, candidateId);
                    }
                }
                else if (e.ColumnIndex == approveIndex)
                {
                    if (dgvSender.Rows[e.RowIndex].Tag != null)
                    {
                        int candidateId = Convert.ToInt32(dgvSender.Rows[e.RowIndex].Tag);
                        string name = dgvSender.Rows[e.RowIndex].Cells["FullName"]?.Value?.ToString() ?? "Unknown";
                        await ApproveCandidateAsync(candidateId, name, dgvSender, cmbStatus);
                    }
                }
                else if (e.ColumnIndex == rejectIndex)
                {
                    if (dgvSender.Rows[e.RowIndex].Tag != null)
                    {
                        int candidateId = Convert.ToInt32(dgvSender.Rows[e.RowIndex].Tag);
                        string name = dgvSender.Rows[e.RowIndex].Cells["FullName"]?.Value?.ToString() ?? "Unknown";
                        await RejectCandidateAsync(candidateId, name, dgvSender, cmbStatus);
                    }
                }
            };

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || s is not DataGridView dgvSender || e.CellStyle == null) return;

                // Alternate row coloring
                if (e.RowIndex % 2 == 0)
                {
                    e.CellStyle.BackColor = Color.FromArgb(250, 250, 250);
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                }

                // Style for Status column
                if (dgvSender.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    e.CellStyle.ForeColor = status switch
                    {
                        "Approved" => COLOR_SUCCESS,
                        "Pending" => COLOR_WARNING,
                        "Rejected" => COLOR_DANGER,
                        _ => Color.Black
                    };
                    e.CellStyle.Font = new Font(dgvSender.Font, FontStyle.Bold);
                }
            };

            cmbStatus.SelectedIndexChanged += async (s, e) =>
            {
                await LoadCandidatesAsync(dgv, cmbStatus.SelectedItem?.ToString() ?? "All Candidates", txtSearch.Text);
            };

            txtSearch.TextChanged += async (s, e) =>
            {
                await LoadCandidatesAsync(dgv, cmbStatus.SelectedItem?.ToString() ?? "All Candidates", txtSearch.Text);
            };

            dgvContainer.Controls.Add(dgv);
            mainPanel.Controls.Add(filterPanel);
            mainPanel.Controls.Add(dgvContainer);
            pnlMainContent.Controls.Add(mainPanel);

            // Load initial data
            _ = LoadCandidatesAsync(dgv, "All Candidates", "");
        }

        private async Task LoadCandidatesAsync(DataGridView dgv, string statusFilter, string search = "")
        {
            try
            {
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                // Use CandidateController endpoints
                string endpoint = statusFilter switch
                {
                    "Pending" => "api/candidate/pending",
                    "Approved" => "api/candidate/approved",
                    "Rejected" => "api/candidate/rejected",
                    _ => "api/candidate/all"
                };

                var response = await _client.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                    // CandidateController returns { success: true, candidates: [...] }
                    if (result.TryGetProperty("success", out var success) && success.GetBoolean())
                    {
                        if (result.TryGetProperty("candidates", out var candidates))
                        {
                            foreach (var candidate in candidates.EnumerateArray())
                            {
                                string fullName = GetJsonString(candidate, "FullName", "fullName", "Name");
                                string email = GetJsonString(candidate, "Email", "email");
                                string party = GetJsonString(candidate, "PartyAffiliation", "Party", "partyAffiliation");
                                string status = GetJsonString(candidate, "Status", "status");
                                string regDate = GetDateTimeString(candidate, "ApplicationDate", "applicationDate");

                                // Check search filter robustly
                                if (!string.IsNullOrEmpty(search))
                                {
                                    if (!fullName.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                                        !email.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                                        !party.Contains(search, StringComparison.OrdinalIgnoreCase))
                                        continue;
                                }

                                int rowIndex = dgv.Rows.Add(fullName, email, party, status, regDate);

                                // Store candidate ID
                                int? candidateId = GetJsonInt(candidate, "Id", "id");
                                if (candidateId.HasValue)
                                    dgv.Rows[rowIndex].Tag = candidateId.Value;

                                // Alternate row colors
                                if (rowIndex % 2 == 0)
                                {
                                    dgv.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                                }

                                // Disable buttons based on status
                                if (status == "Approved")
                                {
                                    dgv.Rows[rowIndex].Cells["btnApprove"].Style.BackColor = Color.LightGray;
                                }
                                else if (status == "Rejected")
                                {
                                    dgv.Rows[rowIndex].Cells["btnReject"].Style.BackColor = Color.LightGray;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to load candidates: {response.StatusCode}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading candidates: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region User Management
        private void CreateUserManagementPanel()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new(20)
            };

            Panel searchPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10)
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(10, 18),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            TextBox txtSearch = new TextBox
            {
                PlaceholderText = "Search by username or email...",
                Location = new Point(90, 15),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };

            Label lblFilter = new()
            {
                Text = "Role:",
                Location = new(310, 18),
                Size = new(80, 20),
                Font = new("Segoe UI", 10, FontStyle.Bold)
            };

            ComboBox cmbRoleFilter = new()
            {
                Location = new(370, 15),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new("Segoe UI", 10)
            };
            cmbRoleFilter.Items.AddRange(_userRoles);
            cmbRoleFilter.SelectedIndex = 0;

            Label lblStatusFilter = new()
            {
                Text = "Status:",
                Location = new(510, 18),
                Size = new(80, 20),
                Font = new("Segoe UI", 10, FontStyle.Bold)
            };

            ComboBox cmbStatusFilter = new()
            {
                Location = new(580, 15),
                Width = 120,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new("Segoe UI", 10)
            };
            cmbStatusFilter.Items.AddRange(_userStatus);
            cmbStatusFilter.SelectedIndex = 0;

            Button btnSearch = new()
            {
                Text = "🔍 Search",
                Location = new(720, 15),
                Size = new(120, 30),
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 9, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(lblFilter);
            searchPanel.Controls.Add(cmbRoleFilter);
            searchPanel.Controls.Add(lblStatusFilter);
            searchPanel.Controls.Add(cmbStatusFilter);
            searchPanel.Controls.Add(btnSearch);

            // Container for DataGridView to ensure proper docking
            Panel gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 20, 0, 0), // Top padding to separate from search
                BackColor = Color.White
            };

            // DataGridView
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(240, 240, 240),
                ColumnHeadersHeight = 45,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            // Set header style
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(0, 5, 0, 5)
            };
            dgv.EnableHeadersVisualStyles = false;

            // Columns
            dgv.Columns.Add("Username", "👤 Username");
            dgv.Columns.Add("Email", "📧 Email Address");
            dgv.Columns.Add("FullName", "📛 Full Name");
            dgv.Columns.Add("Role", "👥 User Role");
            dgv.Columns.Add("RegisteredDate", "📅 Registered Date");
            dgv.Columns.Add("Status", "📊 Account Status");

            // Set column widths
            dgv.Columns["Username"].Width = 120;
            dgv.Columns["Email"].Width = 160;
            dgv.Columns["FullName"].Width = 150;
            dgv.Columns["Role"].Width = 90;
            dgv.Columns["RegisteredDate"].Width = 110;
            dgv.Columns["Status"].Width = 100;

            // Action column
            DataGridViewButtonColumn btnActions = new()
            {
                Name = "btnActions",
                HeaderText = "⚡ Actions",
                Text = "⚡ Manage",
                UseColumnTextForButtonValue = true,
                Width = 100,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnActions);

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex != btnActions.Index) return;

                string username = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString() ?? "";
                string status = dgv.Rows[e.RowIndex].Cells["Status"].Value?.ToString() ?? "Active";
                ShowUserActionsMenu(username, status, dgv, e.RowIndex);
            };

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || s is not DataGridView dgvSender || e.CellStyle == null) return;

                // Alternate row coloring
                e.CellStyle.BackColor = e.RowIndex % 2 == 0 ? Color.FromArgb(250, 250, 250) : Color.White;

                // Style for Role column
                if (dgvSender.Columns[e.ColumnIndex].Name == "Role" && e.Value != null)
                {
                    string role = e.Value.ToString() ?? "";
                    e.CellStyle.ForeColor = role switch
                    {
                        "Admin" => COLOR_DANGER,
                        "Candidate" => COLOR_INFO,
                        _ => COLOR_SECONDARY
                    };
                    e.CellStyle.Font = new(dgvSender.Font, FontStyle.Bold);
                }
                // Style for Status column
                else if (dgvSender.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    e.CellStyle.ForeColor = status == "Active" ? COLOR_SUCCESS : COLOR_DANGER;
                    e.CellStyle.Font = new(dgvSender.Font, FontStyle.Bold);
                }
            };

            // Add grid to container
            gridContainer.Controls.Add(dgv);

            // Add layouts
            mainPanel.Controls.Add(gridContainer);
            mainPanel.Controls.Add(searchPanel); // Search panel on Top

            btnSearch.Click += async (s, e) =>
            {
                await LoadUsersAsync(dgv, txtSearch.Text, cmbRoleFilter.Text, cmbStatusFilter.Text);
            };

            txtSearch.KeyPress += async (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    await LoadUsersAsync(dgv, txtSearch.Text, cmbRoleFilter.Text, cmbStatusFilter.Text);
                }
            };

            cmbRoleFilter.SelectedIndexChanged += async (s, e) =>
            {
                await LoadUsersAsync(dgv, txtSearch.Text, cmbRoleFilter.Text, cmbStatusFilter.Text);
            };

            cmbStatusFilter.SelectedIndexChanged += async (s, e) =>
            {
                await LoadUsersAsync(dgv, txtSearch.Text, cmbRoleFilter.Text, cmbStatusFilter.Text);
            };

            pnlMainContent.Controls.Add(mainPanel);

            // Load initial data
            _ = LoadUsersAsync(dgv, "", "All Roles", "All Status");
        }

        private async Task LoadUsersAsync(DataGridView dgv, string search = "", string roleFilter = "All Roles", string statusFilter = "All Status")
        {
            try
            {
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                var response = await _client.GetAsync("api/admin/all-users");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                    if (result.TryGetProperty("users", out JsonElement users) || result.TryGetProperty("Users", out users))
                    {
                        if (users.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var user in users.EnumerateArray())
                            {
                                string username = GetJsonString(user, "Username", "username");
                                string email = GetJsonString(user, "Email", "email");
                                string fullName = GetJsonString(user, "FullName", "fullName");
                                string role = GetJsonString(user, "Role", "role");
                                string registeredDate = GetDateTimeString(user, "CreatedAt", "createdAt");

                                // Check approval status robustly
                                bool isApproved = false;
                                if (user.TryGetProperty("IsApproved", out var ia)) isApproved = ia.GetBoolean();
                                else if (user.TryGetProperty("isApproved", out ia)) isApproved = ia.GetBoolean();

                                string status = isApproved ? "Active" : "Disabled";

                                // Check status filter
                                if (statusFilter != "All Status" && !status.Equals(statusFilter, StringComparison.OrdinalIgnoreCase))
                                    continue;

                                // Check role filter
                                if (roleFilter != "All Roles" && !role.Equals(roleFilter, StringComparison.OrdinalIgnoreCase))
                                    continue;

                                // Check search filter
                                if (!string.IsNullOrEmpty(search))
                                {
                                    if (!username.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                                        !email.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                                        !fullName.Contains(search, StringComparison.OrdinalIgnoreCase))
                                        continue;
                                }

                                int rowIndex = dgv.Rows.Add(username, email, fullName, role, registeredDate, status);

                                // Store user ID
                                int? userId = GetJsonInt(user, "Id", "id");
                                if (userId.HasValue)
                                    dgv.Rows[rowIndex].Tag = userId.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowUserActionsMenu(string username, string status, DataGridView dgv, int rowIndex)
        {
            ContextMenuStrip menu = new();

            int? userId = dgv.Rows[rowIndex].Tag as int?;
            string fullName = dgv.Rows[rowIndex].Cells["FullName"].Value?.ToString() ?? username;

            ToolStripMenuItem toggleItem = new(
                status == "Active" ? "⏸️ Disable Account" : "▶️ Enable Account");
            toggleItem.Click += async (s, e) =>
            {
                if (userId.HasValue)
                {
                    await ToggleUserAccountAsync(userId.Value, fullName, status == "Active");
                }
            };

            ToolStripMenuItem viewItem = new("👁️ View Details");
            viewItem.Click += (s, e) => ViewUserDetails(fullName, username);

            menu.Items.Add(toggleItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(viewItem);
            menu.Show(Cursor.Position);
        }

        private async Task ToggleUserAccountAsync(int userId, string displayName, bool currentlyActive)
        {
            string action = currentlyActive ? "disable" : "enable";

            var result = MessageBox.Show($"Are you sure you want to {action} account for:\n\n{displayName}?",
                $"Confirm {action.ToUpper()} Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.PutAsync($"api/admin/toggle-user-status/{userId}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"✅ Account {action}d successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshCurrentView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static void ViewUserDetails(string fullName, string username)
        {
            MessageBox.Show($"👤 User Details\n════════════════════════════\n\n" +
                          $"📛 Full Name: {fullName}\n" +
                          $"👤 Username: {username}\n\n" +
                          $"════════════════════════════",
                "User Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Vote Monitoring - FIXED: Proper headers and layout
        private void CreateVoteMonitoringPanel()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new(20)
            };

            // Real-time stats panel
            Panel statsPanel = new()
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 245, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new(15)
            };

            // Total Votes label
            Label lblTotalVotes = new()
            {
                Name = "lblTotalVotes",
                Text = "Total Votes: 0",
                Font = new("Segoe UI", 12, FontStyle.Bold),
                ForeColor = COLOR_PRIMARY,
                Location = new(20, 15),
                AutoSize = true
            };

            // Participation label
            Label lblParticipation = new()
            {
                Name = "lblParticipation",
                Text = "Participation: 0%",
                Font = new("Segoe UI", 12, FontStyle.Bold),
                ForeColor = COLOR_INFO,
                Location = new(200, 15),
                AutoSize = true
            };

            // Last update label
            Label lblLastUpdate = new()
            {
                Name = "lblLastUpdate",
                Text = "Last update: Never",
                Font = new("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new(410, 18),
                AutoSize = true
            };

            // Auto-refresh status
            Label lblRefreshStatus = new()
            {
                Name = "lblRefreshStatus",
                Text = "⏱️ Auto-refresh: ON",
                Font = new("Segoe UI", 9, FontStyle.Italic),
                ForeColor = COLOR_SUCCESS,
                Location = new(410, 38),
                AutoSize = true
            };

            statsPanel.Controls.Add(lblTotalVotes);
            statsPanel.Controls.Add(lblParticipation);
            statsPanel.Controls.Add(lblLastUpdate);
            statsPanel.Controls.Add(lblRefreshStatus);

            // Create DataGridView FIRST
            DataGridView dgvVoteMonitoring = new()
            {
                Name = "dgvVoteMonitoring",
                Dock = DockStyle.Fill,
                Location = new(0, 90),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ColumnHeadersHeight = 45,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(240, 240, 240),
                RowTemplate = { Height = 40 }
            };

            // Set header style
            dgvVoteMonitoring.ColumnHeadersDefaultCellStyle = new()
            {
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                Font = new("Segoe UI", 11, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new(0, 5, 0, 5),
                SelectionBackColor = COLOR_SECONDARY,
                SelectionForeColor = Color.White
            };

            // ADD COLUMNS WITH PROPER HEADERS
            dgvVoteMonitoring.Columns.Add("CandidateName", "Candidate Name");
            dgvVoteMonitoring.Columns.Add("Party", "Party");
            dgvVoteMonitoring.Columns.Add("Votes", "Votes");
            dgvVoteMonitoring.Columns.Add("Percentage", "Percentage");
            dgvVoteMonitoring.Columns.Add("LastUpdateDate", "Last Update Date");

            // Set column widths
            dgvVoteMonitoring.Columns["CandidateName"].Width = 200;
            dgvVoteMonitoring.Columns["Party"].Width = 150;
            dgvVoteMonitoring.Columns["Votes"].Width = 100;
            dgvVoteMonitoring.Columns["Percentage"].Width = 120;
            dgvVoteMonitoring.Columns["LastUpdateDate"].Width = 180;

            // Formatting for columns
            dgvVoteMonitoring.Columns["Percentage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvVoteMonitoring.Columns["Votes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvVoteMonitoring.Columns["LastUpdateDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Row formatting
            dgvVoteMonitoring.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                DataGridView dgvSender = (DataGridView)s!;

                // Alternate row colors
                if (e.RowIndex % 2 == 0)
                {
                    e.CellStyle!.BackColor = Color.FromArgb(250, 250, 250);
                }
                else
                {
                    e.CellStyle!.BackColor = Color.White;
                }

                // Highlight winner (first row)
                if (e.RowIndex == 0 && dgvSender.Rows.Count > 0)
                {
                    e.CellStyle!.BackColor = Color.FromArgb(240, 255, 240);
                    e.CellStyle!.Font = new Font(dgvSender.Font, FontStyle.Bold);
                }

                // Color for Percentage column
                if (dgvSender.Columns[e.ColumnIndex].Name == "Percentage" && e.Value != null)
                {
                    e.CellStyle!.ForeColor = COLOR_INFO;
                    e.CellStyle!.Font = new Font(dgvSender.Font, FontStyle.Bold);
                }
            };

            // Refresh button
            Button btnRefreshVotes = new()
            {
                Text = "🔄 Refresh Now",
                Location = new(600, 15),
                Size = new(130, 35),
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 9, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };

            // Toggle Refresh Button
            Button btnToggleRefresh = new()
            {
                Text = "⏸ PAUSE AUTO",
                Location = new(600, 55),
                Size = new(130, 25),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 8, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };

            // Timer setup
            System.Windows.Forms.Timer refreshTimer = new() { Interval = 10000 };

            btnRefreshVotes.Click += async (s, e) =>
            {
                await LoadVoteMonitoringDataAsync(dgvVoteMonitoring, statsPanel);
            };

            btnToggleRefresh.Click += (s, e) =>
            {
                var lblRefreshStatus = statsPanel.Controls.Find("lblRefreshStatus", true).FirstOrDefault() as Label;
                if (refreshTimer.Enabled)
                {
                    refreshTimer.Stop();
                    btnToggleRefresh.Text = "▶ RESUME AUTO";
                    btnToggleRefresh.BackColor = COLOR_SUCCESS;
                    if (lblRefreshStatus != null)
                    {
                        lblRefreshStatus.Text = "⏱️ Auto-refresh: OFF";
                        lblRefreshStatus.ForeColor = Color.Gray;
                    }
                }
                else
                {
                    refreshTimer.Start();
                    btnToggleRefresh.Text = "⏸ PAUSE AUTO";
                    btnToggleRefresh.BackColor = Color.Gray;
                    if (lblRefreshStatus != null)
                    {
                        lblRefreshStatus.Text = "⏱️ Auto-refresh: ON";
                        lblRefreshStatus.ForeColor = COLOR_SUCCESS;
                    }
                }
            };

            refreshTimer.Tick += async (s, e) => await LoadVoteMonitoringDataAsync(dgvVoteMonitoring, statsPanel);
            refreshTimer.Start();

            statsPanel.Controls.Add(btnRefreshVotes);
            statsPanel.Controls.Add(btnToggleRefresh);
            // Add controls to main panel
            mainPanel.Controls.Add(statsPanel);
            mainPanel.Controls.Add(dgvVoteMonitoring);
            pnlMainContent.Controls.Add(mainPanel);

            // Load initial data
            _ = LoadVoteMonitoringDataAsync(dgvVoteMonitoring, statsPanel);
        }

        private async Task LoadVoteMonitoringDataAsync(DataGridView dgv, Panel statsPanel)
        {
            try
            {
                // Prevent concurrent loading
                if (dgv.Tag != null && dgv.Tag.ToString() == "Loading") return;
                dgv.Tag = "Loading";

                bool isInitialLoad = dgv.Rows.Count == 0;
                if (isInitialLoad) Cursor = Cursors.WaitCursor;

                // 1. Get Overall Statistics
                var statsResponse = await _client.GetAsync("api/admin/vote-statistics");
                if (statsResponse.IsSuccessStatusCode)
                {
                    var statsResult = await statsResponse.Content.ReadFromJsonAsync<JsonElement>();
                    if (statsResult.TryGetProperty("success", out var s) && s.GetBoolean())
                    {
                        int totalVotes = GetJsonInt(statsResult, "totalVotes") ?? 0;
                        double participation = 0;
                        if (statsResult.TryGetProperty("voterTurnout", out var vt))
                            participation = vt.GetDouble();

                        if (statsPanel.Controls.Find("lblTotalVotes", true).FirstOrDefault() is Label lblTotalVotes)
                            lblTotalVotes.Text = $"Total Votes: {totalVotes:N0}";
                        if (statsPanel.Controls.Find("lblParticipation", true).FirstOrDefault() is Label lblParticipation)
                            lblParticipation.Text = $"Participation: {participation:0.0}%";
                        if (statsPanel.Controls.Find("lblLastUpdate", true).FirstOrDefault() is Label lblLastUpdate)
                            lblLastUpdate.Text = $"Last update: {DateTime.Now:HH:mm:ss}";
                    }
                }

                // 2. Get Per-Candidate Results
                var resultsResponse = await _client.GetAsync("api/admin/election-results");
                if (resultsResponse.IsSuccessStatusCode)
                {
                    var resultsJson = await resultsResponse.Content.ReadFromJsonAsync<JsonElement>();

                    if (resultsJson.TryGetProperty("success", out var success) && success.GetBoolean())
                    {
                        if (resultsJson.TryGetProperty("results", out var resultsArray))
                        {
                            // Store current selection
                            int selectedRowIndex = dgv.SelectedRows.Count > 0 ? dgv.SelectedRows[0].Index : -1;

                            dgv.Rows.Clear();

                            int totalVotesInResults = GetJsonInt(resultsJson, "totalVotes") ?? 0;
                            int rowIndex = 0;

                            foreach (var candidate in resultsArray.EnumerateArray())
                            {
                                string name = GetJsonString(candidate, "FullName", "fullName", "CandidateName");
                                string party = GetJsonString(candidate, "PartyAffiliation", "Party", "partyAffiliation");
                                int votes = GetJsonInt(candidate, "Votes", "votes") ?? 0;
                                double percentage = totalVotesInResults > 0 ? (votes * 100.0 / totalVotesInResults) : 0;

                                // Get last update or use current time
                                string lastUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                // Add row with proper columns
                                dgv.Rows.Add(
                                    name,                    // Candidate Name
                                    party,                   // Party
                                    votes.ToString("N0"),    // Votes
                                    $"{percentage:0.0}%",    // Percentage
                                    lastUpdate               // Last Update Date
                                );

                                // Highlight first place
                                if (rowIndex == 0 && votes > 0)
                                {
                                    dgv.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                                    dgv.Rows[rowIndex].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                                }

                                rowIndex++;
                            }

                            // Restore selection
                            if (selectedRowIndex >= 0 && selectedRowIndex < dgv.Rows.Count)
                                dgv.Rows[selectedRowIndex].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Vote monitoring error: {ex.Message}");

                // Show error in stats panel
                if (statsPanel.Controls.Find("lblLastUpdate", true).FirstOrDefault() is Label lblLastUpdate)
                    lblLastUpdate.Text = $"Error: {DateTime.Now:HH:mm:ss}";
            }
            finally
            {
                dgv.Tag = null;
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Election Control
        private void CreateElectionControlPanel()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new(20)
            };

            // Current status panel
            Panel statusPanel = new()
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 245, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new(20)
            };

            Label lblStatusTitle = new()
            {
                Text = "📊 CURRENT ELECTION STATUS",
                Font = new("Segoe UI", 16, FontStyle.Bold),
                ForeColor = COLOR_DARK,
                Location = new(20, 20),
                AutoSize = true
            };

            Label lblStatusValue = new()
            {
                Name = "lblElectionStatus",
                Text = "Loading election status...",
                Font = new("Segoe UI", 18, FontStyle.Bold),
                Location = new(20, 60),
                Size = new(400, 35),
                ForeColor = COLOR_WARNING
            };

            Label lblVotingStatus = new()
            {
                Name = "lblVotingStatus",
                Text = "",
                Font = new("Segoe UI", 14),
                Location = new(450, 65),
                AutoSize = true,
                ForeColor = COLOR_INFO
            };

            statusPanel.Controls.Add(lblStatusTitle);
            statusPanel.Controls.Add(lblStatusValue);
            statusPanel.Controls.Add(lblVotingStatus);

            // Control buttons panel
            Panel controlPanel = new()
            {
                Height = 200,
                Dock = DockStyle.Top,
                Location = new(0, 140),
                Size = new(mainPanel.Width, 200),
                BackColor = Color.White,
                Padding = new(20, 30, 20, 20)
            };

            // Create buttons
            Button btnStartElection = CreateElectionButton("▶ START ELECTION", COLOR_SUCCESS, 20, 20);
            Button btnStopElection = CreateElectionButton("⏹ STOP ELECTION", COLOR_DANGER, 250, 20);
            Button btnToggleVoting = CreateElectionButton("⏯ TOGGLE VOTING", COLOR_INFO, 480, 20);
            Button btnViewResults = CreateElectionButton("📊 VIEW RESULTS", COLOR_SECONDARY, 710, 20);

            // Set button sizes
            btnStartElection.Size = new(200, 60);
            btnStopElection.Size = new(200, 60);
            btnToggleVoting.Size = new(200, 60);
            btnViewResults.Size = new(200, 60);

            // Add event handlers
            btnStartElection.Click += async (s, e) => await StartElectionAsync();
            btnStopElection.Click += async (s, e) => await StopElectionAsync();
            btnToggleVoting.Click += async (s, e) => await ToggleVotingAsync();
            btnViewResults.Click += (s, e) => PanelResults_Click(s, e);

            // Add buttons to control panel
            controlPanel.Controls.Add(btnStartElection);
            controlPanel.Controls.Add(btnStopElection);
            controlPanel.Controls.Add(btnToggleVoting);
            controlPanel.Controls.Add(btnViewResults);

            // Election settings panel
            Panel settingsPanel = new()
            {
                Height = 180,
                Dock = DockStyle.Top,
                Location = new(0, 360),
                Size = new(mainPanel.Width, 180),
                BackColor = Color.FromArgb(250, 250, 255),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new(20, 20, 20, 20)
            };

            Label lblSettingsTitle = new()
            {
                Text = "⚙️ ELECTION SETTINGS",
                Font = new("Segoe UI", 14, FontStyle.Bold),
                ForeColor = COLOR_DARK,
                Location = new(20, 20),
                AutoSize = true
            };

            Label lblDuration = new()
            {
                Text = "Default Election Duration (days):",
                Font = new("Segoe UI", 11),
                Location = new(20, 70),
                Size = new(200, 25),
                ForeColor = Color.DimGray
            };

            NumericUpDown nudDuration = new()
            {
                Location = new(250, 70),
                Size = new(100, 30),
                Minimum = 1,
                Maximum = 30,
                Value = 7,
                Font = new("Segoe UI", 11)
            };

            Button btnSaveSettings = new()
            {
                Text = "💾 SAVE SETTINGS",
                Location = new(380, 70),
                Size = new(150, 30),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 10, FontStyle.Bold)
            };
            btnSaveSettings.FlatAppearance.BorderSize = 0;
            btnSaveSettings.Click += (s, e) =>
            {
                MessageBox.Show($"Election duration set to {nudDuration.Value} days", "Settings Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            settingsPanel.Controls.Add(lblSettingsTitle);
            settingsPanel.Controls.Add(lblDuration);
            settingsPanel.Controls.Add(nudDuration);
            settingsPanel.Controls.Add(btnSaveSettings);

            // Add all panels to main panel
            mainPanel.Controls.Add(statusPanel);
            mainPanel.Controls.Add(controlPanel);
            mainPanel.Controls.Add(settingsPanel);
            pnlMainContent.Controls.Add(mainPanel);

            // Load election status
            _ = LoadElectionControlStatusAsync(lblStatusValue, lblVotingStatus, btnStartElection, btnStopElection, btnToggleVoting);
        }

        private static Button CreateElectionButton(string text, Color color, int x, int y)
        {
            Button btn = new()
            {
                Text = text,
                Location = new(x, y),
                Size = new(200, 60),
                Font = new("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = color,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                FlatAppearance = { BorderSize = 0 }
            };
            return btn;
        }

        private async Task LoadElectionControlStatusAsync(Label lblStatus, Label lblVotingStatus,
            Button btnStart, Button btnStop, Button btnToggle)
        {
            try
            {
                var response = await _client.GetAsync("api/admin/election-status");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    bool isActive = result.GetProperty("isActive").GetBoolean();
                    bool votingOpen = result.GetProperty("votingOpen").GetBoolean();

                    lblStatus.Text = isActive
                        ? (votingOpen ? "✅ ELECTION ACTIVE" : "⏸ ELECTION PAUSED")
                        : "⏸ ELECTION INACTIVE";
                    lblStatus.ForeColor = isActive ? COLOR_SUCCESS : COLOR_WARNING;

                    lblVotingStatus.Text = isActive
                        ? (votingOpen ? "🗳️ Voting: OPEN" : "⏸ Voting: CLOSED")
                        : "";
                    lblVotingStatus.ForeColor = votingOpen ? COLOR_SUCCESS : COLOR_WARNING;

                    btnStart.Enabled = !isActive;
                    btnStop.Enabled = isActive;
                    btnToggle.Enabled = isActive;

                    if (isActive)
                    {
                        btnToggle.Text = votingOpen ? "⏸ PAUSE VOTING" : "▶ RESUME VOTING";
                        btnToggle.BackColor = votingOpen ? COLOR_WARNING : COLOR_SUCCESS;
                    }
                }
            }
            catch
            {
                lblStatus.Text = "❌ ERROR LOADING STATUS";
                lblStatus.ForeColor = COLOR_DANGER;
                lblVotingStatus.Text = "Failed to load voting status";
            }
        }

        private Task StartElectionAsync()
        {
            using var startForm = new Form();
            startForm.Text = "Start Election";
            startForm.Size = new(500, 350);
            startForm.StartPosition = FormStartPosition.CenterParent;
            startForm.FormBorderStyle = FormBorderStyle.FixedDialog;

            Label lblEndDate = new()
            {
                Text = "⏰ Election End Date & Time:",
                Location = new(20, 100),
                Size = new(200, 25),
                Font = new("Segoe UI", 10, FontStyle.Bold)
            };

            DateTimePicker dtpEndDate = new()
            {
                Location = new(230, 100),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                ShowUpDown = true,
                Value = DateTime.Now.AddDays(7),
                MinDate = DateTime.Now.AddHours(1)
            };

            Button btnConfirm = new()
            {
                Text = "🚀 START ELECTION",
                Location = new(150, 200),
                Size = new(200, 40),
                BackColor = COLOR_SUCCESS,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 11, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };

            btnConfirm.Click += async (s, e) =>
            {
                var request = new { EndTime = dtpEndDate.Value };
                var response = await _client.PostAsJsonAsync("api/admin/start-election", request);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("✅ Election started successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    startForm.Close();
                    await UpdateElectionStatusHeaderAsync();
                    RefreshCurrentView();
                }
            };

            startForm.Controls.Add(lblEndDate);
            startForm.Controls.Add(dtpEndDate);
            startForm.Controls.Add(btnConfirm);
            startForm.ShowDialog();
            return Task.CompletedTask;
        }

        private async Task StopElectionAsync()
        {
            var result = MessageBox.Show("Are you sure you want to stop the election?\nThis will end voting immediately.",
                "Confirm Stop Election", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.PostAsync("api/admin/stop-election", null);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("⏹ Election stopped successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await UpdateElectionStatusHeaderAsync();
                        RefreshCurrentView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task ToggleVotingAsync()
        {
            var result = MessageBox.Show("Are you sure you want to toggle voting status?",
                "Confirm Toggle Voting", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var response = await _client.PostAsync("api/admin/toggle-voting", null);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadFromJsonAsync<JsonElement>();
                        string message = resultJson.TryGetProperty("message", out var msg) ? msg.GetString() ?? "Success" : "Success";
                        MessageBox.Show(message, "Voting Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await UpdateElectionStatusHeaderAsync();
                        RefreshCurrentView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static async Task ShowCandidateDetailsAsync(HttpClient client, int candidateId)
        {
            try
            {
                var response = await client.GetAsync($"api/admin/candidate-details/{candidateId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    if (result.TryGetProperty("success", out var s) && s.GetBoolean())
                    {
                        var candidate = result.GetProperty("candidate");
                        string details = FormatCompleteCandidateDetails(candidate);

                        MessageBox.Show(details, "Candidate Profile",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Results Module
        private void CreateResultsPanel()
        {
            Panel mainPanel = new()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new(20)
            };

            Label lblStatusCheck = new()
            {
                Text = "Checking election status...",
                Font = new("Segoe UI", 14),
                Location = new(20, 20),
                AutoSize = true,
                ForeColor = COLOR_WARNING
            };

            mainPanel.Controls.Add(lblStatusCheck);
            pnlMainContent.Controls.Add(mainPanel);

            _ = LoadResultsPanelAsync(mainPanel, lblStatusCheck);
        }

        private async Task LoadResultsPanelAsync(Panel mainPanel, Label lblStatusCheck)
        {
            try
            {
                var response = await _client.GetAsync("api/admin/election-status");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();
                    bool isActive = result.TryGetProperty("isActive", out var ia) && ia.GetBoolean();

                    mainPanel.Controls.Remove(lblStatusCheck);

                    if (isActive)
                    {
                        ShowElectionActiveMessage(mainPanel);
                    }
                    else
                    {
                        CreateResultsGrid(mainPanel);
                    }
                }
            }
            catch
            {
                lblStatusCheck.Text = "Error checking election status";
                lblStatusCheck.ForeColor = COLOR_DANGER;
            }
        }

        private static void ShowElectionActiveMessage(Panel mainPanel)
        {
            Label lblMessage = new()
            {
                Text = "📊 ELECTION RESULTS\n════════════════════════════\n\n" +
                       "Results are available only after the election ends.\n" +
                       "Current election status: ACTIVE\n\n" +
                       "Please wait for the election to finish or\n" +
                       "stop the election from Election Control panel.",
                Font = new("Segoe UI", 16),
                Location = new(50, 50),
                Size = new(600, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = COLOR_WARNING
            };

            mainPanel.Controls.Add(lblMessage);
        }

        private void CreateResultsGrid(Panel mainPanel)
        {
            Label lblTitle = new()
            {
                Text = "🏆 ELECTION RESULTS - FINAL",
                Font = new("Segoe UI", 24, FontStyle.Bold),
                ForeColor = COLOR_DARK,
                Location = new(20, 20),
                AutoSize = true
            };

            DataGridView dgv = new()
            {
                Location = new(20, 80),
                Size = new(mainPanel.Width - 60, mainPanel.Height - 150),
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                ColumnHeadersHeight = 45,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            // Set header style
            dgv.ColumnHeadersDefaultCellStyle = new()
            {
                BackColor = COLOR_SECONDARY,
                ForeColor = Color.White,
                Font = new("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new(0, 5, 0, 5)
            };
            dgv.EnableHeadersVisualStyles = false;

            dgv.Columns.Add("Position", "🏅 Position");
            dgv.Columns.Add("Candidate", "👤 Candidate");
            dgv.Columns.Add("Party", "🏛️ Party");
            dgv.Columns.Add("Votes", "🗳️ Votes");
            dgv.Columns.Add("Percentage", "📊 Percentage");
            dgv.Columns.Add("Status", "📈 Status");

            // Export Button
            Button btnExport = new()
            {
                Text = "📥 EXPORT RESULTS",
                Location = new(mainPanel.Width - 200, 25),
                Size = new(160, 35),
                BackColor = COLOR_SUCCESS,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new("Segoe UI", 9, FontStyle.Bold),
                FlatAppearance = { BorderSize = 0 }
            };
            btnExport.Click += (s, e) => ExportResults(dgv);

            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(btnExport);
            mainPanel.Controls.Add(dgv);
            _ = LoadResultsDataAsync(dgv);
        }

        private static void ExportResults(DataGridView dgv)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No results to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            System.Text.StringBuilder sb = new();
            sb.AppendLine("📜 OFFICIAL ELECTION RESULTS");
            sb.AppendLine("══════════════════════════════");
            sb.AppendLine($"Exported on: {DateTime.Now:f}\n");

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;
                string pos = row.Cells["Position"].Value?.ToString() ?? "";
                string name = row.Cells["Candidate"].Value?.ToString() ?? "";
                string party = row.Cells["Party"].Value?.ToString() ?? "";
                string votes = row.Cells["Votes"].Value?.ToString() ?? "";
                string pct = row.Cells["Percentage"].Value?.ToString() ?? "";
                string status = row.Cells["Status"].Value?.ToString() ?? "";

                sb.AppendLine($"{pos.PadRight(8)} {name.PadRight(20)} {party.PadRight(15)} {votes.PadLeft(6)} votes ({pct}) - {status}");
            }

            sb.AppendLine("\n══════════════════════════════");
            sb.AppendLine("Verified by ETH Election System");

            // In a real scenario, this would save to a .txt or .csv
            // For now, we show it in a formatted dialog for the admin to copy
            Form reportForm = new()
            {
                Text = "Election Results Report",
                Size = new(600, 500),
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox txtReport = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                Text = sb.ToString(),
                ScrollBars = ScrollBars.Vertical
            };

            reportForm.Controls.Add(txtReport);
            reportForm.ShowDialog();
        }

        private async Task LoadResultsDataAsync(DataGridView dgv)
        {
            try
            {
                dgv.Rows.Clear();
                Cursor = Cursors.WaitCursor;

                // Get results from AdminController
                var response = await _client.GetAsync("api/admin/election-results");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                    if (result.TryGetProperty("success", out var success) && success.GetBoolean())
                    {
                        if (result.TryGetProperty("results", out var results))
                        {
                            int position = 1;
                            int totalVotes = result.TryGetProperty("totalVotes", out var tv) ? tv.GetInt32() : 0;

                            foreach (var candidate in results.EnumerateArray())
                            {
                                int votes = GetJsonInt(candidate, "Votes", "votes") ?? 0;
                                double percentage = totalVotes > 0 ? (votes * 100.0 / totalVotes) : 0;

                                string positionText = position switch
                                {
                                    1 => "🏆 1st",
                                    2 => "🥈 2nd",
                                    3 => "🥉 3rd",
                                    _ => $"{position}th"
                                };

                                string statusText = position == 1 ? "WINNER 🏆" : "Runner-up";

                                dgv.Rows.Add(
                                    positionText,
                                    GetJsonString(candidate, "FullName", "fullName"),
                                    GetJsonString(candidate, "PartyAffiliation", "Party", "partyAffiliation"),
                                    votes,
                                    $"{percentage:0.0}%",
                                    statusText
                                );

                                // Highlight winner
                                if (position == 1)
                                {
                                    dgv.Rows[0].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                                    dgv.Rows[0].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
                                }

                                position++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading results: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Helper Methods
        private static string FormatCompleteCandidateDetails(JsonElement candidate)
        {
            return $@"👤 COMPLETE CANDIDATE DETAILS
════════════════════════════════════════════════════════════════

📛 BASIC INFORMATION:
   Full Name:       {GetJsonString(candidate, "FullName", "fullName")}
   Age:             {GetJsonString(candidate, "Age", "age")} years
   Region:          {GetJsonString(candidate, "Region", "region")}
   Party:           {GetJsonString(candidate, "PartyAffiliation", "partyAffiliation")}

📞 CONTACT INFORMATION:
   Email:           {GetJsonString(candidate, "Email", "email")}
   Phone:           {GetJsonString(candidate, "Phone", "phone")}
   Address:         N/A

📊 APPLICATION STATUS:
   Status:          {GetJsonString(candidate, "Status", "status")}
   Approved:        {GetJsonString(candidate, "IsApproved", "isApproved")}
   Application:     {GetJsonString(candidate, "ApplicationDate", "applicationDate")}
   Approval Date:   {GetJsonString(candidate, "ApprovalDate", "approvalDate")}

👤 USER ACCOUNT:
   Username:        {GetJsonString(candidate, "Username", "username")}
   User Role:       {GetJsonString(candidate, "UserRole", "userRole")}

🎓 QUALIFICATIONS:
   Education:       N/A
   Experience:      N/A

🗳️ VOTING STATISTICS:
   Votes Received:  {GetJsonString(candidate, "VotesReceived", "votesReceived")}
   Vote Percentage: {GetJsonString(candidate, "VotePercentage", "votePercentage")}%

════════════════════════════════════════════════════════════════
Candidate ID: {GetJsonString(candidate, "Id", "id")}";
        }

        private async Task ApproveCandidateAsync(int candidateId, string name, DataGridView dgv, ComboBox cmbStatus)
        {
            var result = MessageBox.Show($"Approve candidate: {name}?",
                "Confirm Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Use CandidateController endpoint
                    var response = await _client.PutAsync($"api/candidate/approve/{candidateId}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("✅ Candidate approved successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadCandidatesAsync(dgv, cmbStatus?.SelectedItem?.ToString() ?? "All Candidates");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task RejectCandidateAsync(int candidateId, string name, DataGridView dgv, ComboBox cmbStatus)
        {
            var result = MessageBox.Show($"Reject candidate: {name}?",
                "Confirm Rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Use CandidateController endpoint
                    var response = await _client.PutAsync($"api/candidate/reject/{candidateId}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("❌ Candidate rejected!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadCandidatesAsync(dgv, cmbStatus?.SelectedItem?.ToString() ?? "All Candidates");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // JSON helper methods
        private static string GetJsonString(JsonElement element, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                if (element.TryGetProperty(propertyName, out var prop))
                {
                    if (prop.ValueKind == JsonValueKind.String)
                        return prop.GetString() ?? "N/A";
                    else if (prop.ValueKind == JsonValueKind.Number)
                        return prop.GetInt32().ToString();
                    else if (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False)
                        return prop.GetBoolean() ? "Yes" : "No";
                }
            }
            return "N/A";
        }

        private static int? GetJsonInt(JsonElement element, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                if (element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number)
                    return prop.GetInt32();
            }
            return null;
        }

        private static string GetDateTimeString(JsonElement element, params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                if (element.TryGetProperty(propertyName, out var prop) && prop.TryGetDateTime(out var date))
                    return date.ToString("yyyy-MM-dd");
            }
            return "N/A";
        }

        private void Logout()
        {
            UserSession.Clear();
            this.Hide();

            FrmLogin loginForm = new();
            loginForm.FormClosed += (s, e) => this.Close();
            loginForm.Show();
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Logout();
            }
        }
        #endregion

        #region Keyboard Shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                case Keys.Control | Keys.R:
                    RefreshCurrentView();
                    return true;

                case Keys.Control | Keys.D:
                    PanelDashboard_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.C:
                    PanelCandidate_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.U:
                    PanelUserManagement_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.V:
                    PanelVoteMonitoring_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.E:
                    PanelElectionControl_Click(this, EventArgs.Empty);
                    return true;

                case Keys.Control | Keys.L:
                    Logout();
                    return true;

                case Keys.Escape:
                    if (MessageBox.Show("Close application?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.Close();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        private void Label3_Click(object? sender, EventArgs e)
        {
            // This method is kept for compatibility with the designer
        }

        private void PanelCandidate_Paint(object? sender, PaintEventArgs e)
        {
            // This method is kept for compatibility with the designer
        }
    }
}